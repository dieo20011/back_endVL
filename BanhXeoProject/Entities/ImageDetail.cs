namespace BanhXeoProject.Entities
{
    public class ImageDetail
    {
        public int Id { get; set; }
        public string ImgLink { get; set; }
        public string Desc { get; set; }
        public int GroupId { get; set; }
        public virtual GroupImage? GroupImage { get; set; }
    }
}
