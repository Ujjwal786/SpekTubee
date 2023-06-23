namespace SpekTube.Models
{
    public class VideoComment
    {
        public int Id { get; set; }
        public int UserID_FK { get; set; }
        public int VideoID_FK { get; set; }
        public string Comment { get; set; }
        public bool IsReply { get; set; }
        public DateTime Created_At { get; set; }

    }
}
