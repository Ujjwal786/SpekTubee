namespace SpekTube.Models
{
    public class Channels
    {
        public int Id { get; set; }
        public int UserID_FK { get; set; }
        public string? Channel_Logo_Url { get; set; }
        public string? Channel_Name { get; set; }
        public string? Channel_Description { get; set; }

    }

}
