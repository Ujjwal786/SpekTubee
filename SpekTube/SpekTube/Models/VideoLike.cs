namespace SpekTube.Models
{
    public class VideoLike
    {
        public int Id { get; set; }
        public int VideoID_FK { get; set; }
        public int UserID_FK { get; set; }
        public int IsLikeDislike { get; set; }

    }
}
