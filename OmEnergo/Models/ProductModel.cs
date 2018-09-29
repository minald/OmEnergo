﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OmEnergo.Models
{
    public class ProductModel : CommonObject
    {
        public Section Section { get; set; }

        public Product Product { get; set; }

        [Display(Name = "Цена")]
        public double Price { get; set; }

        [Display(Name = "Свойства")]
        public string Properties { get; set; } //Json object in DB

        public ProductModel() { }

        public ProductModel(Section section, Product product)
        {
            Section = section;
            Product = product;
            UpdateProperties((section ?? product.Section).GetProductModelPropertiesList());
        }

        public IEnumerable<ProductProperty> GetProperties()
        {
            var properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(Properties);
            foreach (var pair in properties)
            {
                yield return new ProductProperty(pair.Key, pair.Value);
            }
        }

        public string GetPropertyNames() => String.Join(';', GetProperties().Select(x => x.DisplayName));

        public void UpdateProperties(List<string> propertyNames)
        {
            var result = new Dictionary<string, string>();
            var properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(Properties ?? "{}");
            foreach (var propertyName in propertyNames)
            {
                var propertyPair = properties.FirstOrDefault(x => x.Key == propertyName);
                result.Add(propertyPair.Key ?? propertyName, propertyPair.Value ?? String.Empty);
            }

            Properties = JsonConvert.SerializeObject(result);
        }

        public override string GetImageFullLink()
        {
            var productName = Product.Name.Replace('"', '\'');
            return $"/images/{Product.Section.Name}/{productName}/{Name.Replace('/', '-')}.jpg";
        }

        public string GetStringPriceFractionalPart()
        {
            string prefix = GetPriceFractionalPart() < 10 ? "0" : "";
            return prefix + GetPriceFractionalPart().ToString();
        }

        private int GetPriceFractionalPart() => (int)(Math.Round((Price - GetPriceIntegerPart()) * 100));

        public int GetPriceIntegerPart() => (int)Price;
    }
}
