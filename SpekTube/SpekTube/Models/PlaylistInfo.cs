namespace SpekTube.Models
{
    public class PlaylistInfo
    {
        public int Id { get; set; }
        public string Playlist_Name { get; set; }
        public string ThumbnailURL { get; set; }
        public int VideoCount { get; set; }
        public DateTime LastDate { get; set; }
    }

}
