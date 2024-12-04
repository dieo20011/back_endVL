using BanhXeoProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanhXeoProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        [NonAction]
        public IActionResult ErrorResult(string errorMessages)
        {
            var dataResult = new ErrorResponseModel
            {
                Status = false,
                ErrorMessage = errorMessages,
            };
            return Ok(dataResult);
        }

        /// <summary>
        /// prepare success result
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        public IActionResult SuccessResult(string message)
        {
            var dataResult = new SuccessResponseModel<object>
            {
                Status = true,
                Message = message,
            };
            return Ok(dataResult);
        }

        /// <summary>
        /// prepare success result
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        public IActionResult SuccessResult(object obj, string message = "")
        {
            var dataResult = new SuccessResponseModel<object>
            {
                Status = true,
                Message = message,
                Data = obj,
            };
            return Ok(dataResult);
        }

        /// <summary>
        /// File Result
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        [NonAction]
        public IActionResult FileResult(byte[] file, string fileName, string contentType)
        {
            if (file == null) return ErrorResult("No results were found for your selections.");
            return File(file, contentType, fileName);
        }
    }
}
