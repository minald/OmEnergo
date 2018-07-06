using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string EnglishName { get; set; }

        public string RussianName { get; set; }

        public string MainImageLink { get; set; }

        public string Description { get; set; }

        public string GetImageFullLink() => $"/images/{EnglishName}/{MainImageLink}";
    }
}
