namespace SpekTube.Models
{
    public class VideoData
    {
        public int VideoID { get; set; }
        public string VideoTitle { get; set; }
        public string VideoDescription { get; set; }
        public string DaysAgoFormatted { get; set; }
        public int UserID { get; set; }
        public string VideoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ChannelName { get; set; }
        public string ChannelLogoUrl { get; set; }
        public int Views { get; set; }
    }



}
