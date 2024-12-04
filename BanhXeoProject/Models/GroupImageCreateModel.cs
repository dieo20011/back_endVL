namespace BanhXeoProject.Models
{
    public class GroupImageCreateModel
    {
        public string Date { get; set; } = null!;
        public string Desc { get; set; } = null!;
        public IFormFileCollection files { get; set; } = new FormFileCollection();
    }
}
