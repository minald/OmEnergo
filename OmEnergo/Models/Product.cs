using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public abstract class Product
    {
        [Key]
        public int Id { get; set; }

        public Section Section { get; set; }

        public string Series { get; set; } //Enum in the future

        public string MainImageLink { get; set; }

        public string GetImageFullLink() => $"/images/{Section.Name}/{Series.Replace('"', '\'')}/{MainImageLink}";
    }
}
