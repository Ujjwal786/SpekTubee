using System.ComponentModel.DataAnnotations;

namespace SpekTube.Models
{
    public class VideoDescription
    {
        [Key]
        public int Id {  get; set; }
        public int VideoID_FK { get; set; }
        public int PlayListID_FK { get; set; }
        public int CategoryID_FK { get; set; }
        public string? Video_Title { get; set; }
        public string? Video_Description { get; set; }
        public string? Video_Scope {  get; set; }
        //public DateTime created_at { get;  internal set; }
    }
}
