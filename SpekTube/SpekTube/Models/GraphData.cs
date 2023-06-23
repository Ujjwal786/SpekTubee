using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SpekTube.Models
{
    public class GraphData
    {
        [Key]
        public string MonthName { get; set; }
        public int Total_Views { get; set; }
        public int Total_Likes { get; set; }
        public int Total_Dislikes { get; set; }
        public int Total_Subscribers { get; set; }
    }
}
