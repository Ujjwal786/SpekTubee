namespace SpekTube.Models
{
    public class VideoThumbnail
    {
        public int Id { get; set; }
        public int VideoID_FK { get; set; }
        public string? Thumbnail_File { get; set; }
        public string? Thumbnail_Url { get; set; }

    }
}
