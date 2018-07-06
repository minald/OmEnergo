using System;
using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class AutotransformerModel
    {
        [Key]
        public int Id { get; set; }

        public Autotransformer Autotransformer { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Цена")]
        public double Price { get; set; }

        [Display(Name = "Габариты, мм")]
        public string Dimensions { get; set; } //Class in the future, e.g. ("500x100x100 mm")

        [Display(Name = "Вес, кг")]
        public double? Weight { get; set; }

        public string GetImageFullLink()
        {
            var stabilizerSeries = Autotransformer.Series.Replace('"', '\'');
            return $"/images/{Autotransformer.Product.EnglishName}/{stabilizerSeries}/{Name.Replace('/', '-')}.jpg";
        }

        public int GetPriceIntegerPart() => (int)Price;

        public int GetPriceFractionalPart() => (int)(Math.Round((Price - GetPriceIntegerPart()) * 100));

        public string GetStringPriceFractionalPart()
        {
            string prefix = GetPriceFractionalPart() < 10 ? "0" : "";
            return prefix + GetPriceFractionalPart().ToString();
        }
    }
}
