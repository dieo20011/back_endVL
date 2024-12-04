namespace BanhXeoProject.Entities
{
    public class GroupImage
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public virtual ICollection<ImageDetail> ImageDetail { get; set; } = new List<ImageDetail>();

    }
}
