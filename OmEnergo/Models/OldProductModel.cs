using System;
using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class OldProductModel<T> where T : OldProduct
    {
        [Key]
        public int Id { get; set; }

        public T Product { get; set; }

        public string Name { get; set; }

        [Display(Name = "Цена")]
        public double Price { get; set; }

        [Display(Name = "Габариты, мм")]
        public string Dimensions { get; set; } //Class in the future, e.g. ("500x100x100 mm")

        [Display(Name = "Вес, кг")]
        public double? Weight { get; set; }

        public string GetImageFullLink()
        {
            var series = Product.Series.Replace('"', '\'');
            return $"/images/{Product.Section.Name}/{series}/{Name.Replace('/', '-')}.jpg";
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
