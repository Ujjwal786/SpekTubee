namespace SpekTube.Models
{
    public class VideoReport
    {
        public int Id { get; set; }
        public int UserID_FK { get; set; }
        public int VideoID_FK { get; set; }
        public string Problem { get; set; }
    }
}
