using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class StabilizerModel
    {
        [Key]
        public int Id { get; set; }

        public Stabilizer Stabilizer { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Цена")]
        public double? Price { get; set; }

        [Display(Name = "Габариты")]
        public string Dimensions { get; set; } //Class in the future, e.g. ("500x100x100 mm")

        [Display(Name = "Вес, кг")]
        public float? Weight { get; set; } 

        public string GetImageFullLink()
        {
            var stabilizerSeries = Stabilizer.Series.Replace('"', '\'');
            return $"/images/{Stabilizer.Product.EnglishName}/{stabilizerSeries}/{Name}.jpg";
        }
    }
}
