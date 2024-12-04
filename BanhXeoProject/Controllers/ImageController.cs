using BanhXeoProject.Data;
using BanhXeoProject.Entities;
using BanhXeoProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace BanhXeoProject.Controllers
{
    public class ImageController : BaseApiController
    {
        private readonly AppDbContext _context;
        private readonly string _imageDirectory = "e3b6f53b4c36b25";

        public ImageController(AppDbContext context)
        {
            _context = context;

            // Ensure the image directory exists
            if (!Directory.Exists(_imageDirectory))
            {
                Directory.CreateDirectory(_imageDirectory);
            }
        }

        // POST: api/ImageGallery
        [HttpPost]
        public async Task<IActionResult> PostGroupImage([FromForm] GroupImageCreateModel model)
        {
            // Add Group Image
            var group = new GroupImage()
            {
                Date = model.Date,
            };

            // Imgur API client setup
            var imgurClientId = "e3b6f53b4c36b25"; // Use your actual Imgur Client ID
            var imgurApiUrl = "https://api.imgur.com/3/image";

            // Process uploaded files
            foreach (var file in model.files)
            {
                if (file.Length > 0)
                {
                    // Create an HTTP client to send the image to Imgur
                    using (var client = new HttpClient())
                    {
                        // Set the authorization header with the Imgur Client ID
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", imgurClientId);

                        // Prepare the file to upload
                        var formData = new MultipartFormDataContent();
                        var streamContent = new StreamContent(file.OpenReadStream());
                        formData.Add(streamContent, "image", file.FileName);

                        try
                        {
                            var response = await client.PostAsync(imgurApiUrl, formData);
                            response.EnsureSuccessStatusCode();

                            var responseContent = await response.Content.ReadAsStringAsync();
                            dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);
                            string imgurLink = jsonResponse.data.link;

                            // Create ImageDetail for the file with the Imgur URL
                            var imageDetail = new Entities.ImageDetail
                            {
                                ImgLink = imgurLink, // Use the Imgur link returned in the response
                                Desc = model.Desc, // Or use a parameter to pass description
                            };

                            // Add ImageDetail to the group
                            group.ImageDetail.Add(imageDetail);
                        }
                        catch (Exception ex)
                        {
                            return BadRequest($"Failed to upload image to Imgur: {ex.Message}");
                        }
                    }
                }
            }

            // Add the group with its details to the context
            _context.GroupImages.Add(group);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return SuccessResult("Add Memories success");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupImage(int id, [FromForm] GroupImageUpdateModel model)
        {
            var group = await _context.GroupImages.Include(g => g.ImageDetail)
                                                   .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
            {
                return NotFound($"GroupImage with ID {id} not found.");
            }

            group.Date = model.Date;

            List<int> imageIdsToKeep = new List<int>();
            if (!string.IsNullOrEmpty(model.Images))
            {
                imageIdsToKeep = model.Images.Split(',')
                                              .Select(id => int.TryParse(id, out var parsedId) ? parsedId : 0)
                                              .Where(id => id > 0)
                                              .ToList();
                foreach (var imageDetail in group.ImageDetail)
                {
                 if (imageIdsToKeep.Contains(imageDetail.Id))
                    {
                        imageDetail.Desc = model.Desc;
                    }
                }
            }

            var imagesToDelete = group.ImageDetail.Where(img => !imageIdsToKeep.Contains(img.Id)).ToList();
            foreach (var image in imagesToDelete)
            {
                group.ImageDetail.Remove(image);
            }

            if (model.files != null && model.files.Any())
            {
                var imgurClientId = "e3b6f53b4c36b25"; // Imgur Client ID
                var imgurApiUrl = "https://api.imgur.com/3/image";

                foreach (var file in model.files)
                {
                    if (file.Length > 0)
                    {
                        using (var client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", imgurClientId);

                            var formData = new MultipartFormDataContent();
                            var streamContent = new StreamContent(file.OpenReadStream());
                            formData.Add(streamContent, "image", file.FileName);

                            try
                            {
                                var response = await client.PostAsync(imgurApiUrl, formData);
                                response.EnsureSuccessStatusCode();

                                var responseContent = await response.Content.ReadAsStringAsync();
                                dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);
                                string imgurLink = jsonResponse.data.link;

                                // Tạo đối tượng ImageDetail cho hình ảnh mới
                                var imageDetail = new Entities.ImageDetail
                                {
                                    ImgLink = imgurLink,
                                    Desc = model.Desc,
                                };

                                // Thêm ImageDetail vào nhóm
                                group.ImageDetail.Add(imageDetail);
                            }
                            catch (Exception ex)
                            {
                                return BadRequest($"Failed to upload image to Imgur: {ex.Message}");
                            }
                        }
                    }
                }
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.GroupImages.Update(group);
            await _context.SaveChangesAsync();

            return SuccessResult("GroupImage updated successfully.");
        }


        // GET: api/ImageGallery
        [HttpGet]
        public async Task<IActionResult> GetGroupImages()
        {
            var result = await _context.GroupImages.Select(t => new GroupImageResponse
            {
                Id = t.Id,
                Date = t.Date,
                Desc = t.ImageDetail.FirstOrDefault()!.Desc,
                imageDetails = t.ImageDetail.Select(y => new Models.ImageDetail
                {
                    Id = y.Id,  
                    ImageLink = y.ImgLink
                }).ToList()
            }).ToListAsync();
            return SuccessResult(result);
        }

        // GET: api/ImageGallery/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupImage(int id)
        {
            var groupImage = await _context.GroupImages
                .Where(g => g.Id == id)
                .Select(t => new GroupImageResponse
                {
                    Id = t.Id,
                    Date = t.Date,
                    Desc = t.ImageDetail.FirstOrDefault()!.Desc,
                    imageDetails = t.ImageDetail.Select(y => new Models.ImageDetail
                    {
                        Id = y.Id,
                        ImageLink = y.ImgLink
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (groupImage == null)
            {
                return NotFound();
            }

            return SuccessResult(groupImage);
        }

        [HttpDelete]
        [Route("group/{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var x = await _context.GroupImages.FindAsync(id);
            if(x == null)
            {
                return ErrorResult("Not found");
            }
            if(x.ImageDetail.Count > 0)
            {
                _context.RemoveRange(x.ImageDetail);
            }
            _context.Remove(x);
            await _context.SaveChangesAsync();
            return SuccessResult("Success");
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteImage([FromRoute] int id)
        {
            var x = await _context.ImageDetails.FindAsync(id);
            if (x == null)
            {
                return ErrorResult("Not found");
            }
            _context.Remove(x);
            await _context.SaveChangesAsync();
            return SuccessResult("Success");
        }
    }
}
