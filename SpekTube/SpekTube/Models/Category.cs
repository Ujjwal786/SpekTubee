using System.ComponentModel.DataAnnotations;

namespace SpekTube.Models
{
    public class Category
    {
        public int id { get; set; }

        [Required]
        public string? category_name { get; set; }

    }

}
