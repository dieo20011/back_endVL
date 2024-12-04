namespace BanhXeoProject.Models
{
    public class GroupImageUpdateModel
    {
        public string Date { get; set; } = string.Empty;
        public string Desc { get; set; } = string.Empty;
        public string Images { get; set; } = string.Empty;
        public IFormFileCollection files { get; set; } = new FormFileCollection();
    }
}
