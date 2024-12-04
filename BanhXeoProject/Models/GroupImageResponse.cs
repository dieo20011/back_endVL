namespace BanhXeoProject.Models
{
    public class GroupImageResponse
    {
        public int Id { get; set; } 
        public string Date { get; set; } = string.Empty;
        public string Desc { get; set; } = string.Empty;
        public List<ImageDetail> imageDetails { get; set; } = new List<ImageDetail>();
    }
    public class ImageDetail
    {
        public int Id { get; set; }
        public string ImageLink { get; set; } = string.Empty;
    }
}
