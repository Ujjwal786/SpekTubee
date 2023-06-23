using System.ComponentModel.DataAnnotations;

namespace SpekTube.Models
{
    public class VideoWatchHistory
    {
        [Key]
        public int Id { get; set; }
        public int VideoID_FK { get; set; }

        public int UserID_FK { get; set; }
    }
}
