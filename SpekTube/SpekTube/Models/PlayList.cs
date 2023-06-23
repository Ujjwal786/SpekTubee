namespace SpekTube.Models
{
    public class PlayList
    {
        public int ID { get; set; }
        public int UserID_FK { get; set; }
        public string PlayList_Name { get; set; }
    }
}
