using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace OmEnergo.Models
{
    public class CommonProductModel
    {
        public int Id { get; set; }

        public CommonProduct CommonProduct { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string Properties { get; set; }

        public IEnumerable<ProductProperty> GetProperties()
        {
            var properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(Properties);
            foreach (var pair in properties)
            {
                yield return new ProductProperty(pair.Key, pair.Value);
            }
        }

        public string GetImageFullLink()
        {
            var productName = CommonProduct.Name.Replace('"', '\'');
            return $"/images/{CommonProduct.Section.EnglishName}/{productName}/{Name.Replace('/', '-')}.jpg";
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
