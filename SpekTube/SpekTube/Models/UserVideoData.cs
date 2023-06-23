namespace SpekTube.Models
{
    public class UserVideoData
    {
        public int id { get; set; }
        public string video_title { get; set; }
        public string video_description { get; set; }
        public string video_url { get; set; }
        public string thumbnail_url { get; set; }
        public string video_scope { get; set; }
        public int views { get; set; }
        public int comments { get; set; }
        public int likes { get; set; }
        public int dislikes { get; set; }
        public DateTime created_at { get; set; }
    }
}
