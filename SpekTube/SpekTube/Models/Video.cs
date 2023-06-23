namespace SpekTube.Models
{
    public class Video
    {
        public int Id { get; set; }
        public int UserID_FK { get; set; }
        public string? Video_File_Name { get; set; }
        public string? Video_Url { get; set; }

    }
}
