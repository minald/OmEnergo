using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OmEnergo.Models
{
    public class Section : CommonObject
    {
        public Section ParentSection { get; set; }

        public List<Section> ChildSections { get; set; }

        public List<Product> Products { get; set; }

        public List<ProductModel> ProductModels { get; set; }

        [Display(Name = "Ссылка на фото")]
        public string MainImageLink { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Свойства продукта")]
        public string ProductProperties { get; set; } //Json array in DB

        [Display(Name = "Свойства модели продукта")]
        public string ProductModelProperties { get; set; } //Json array in DB

        public Section()
        {
            ProductProperties = "[]";
            ProductModelProperties = "[]";
        }

        public IEnumerable<CommonObject> GetNestedObjects()
        {
            var list = new List<CommonObject>();
            list.AddRange(ChildSections ?? new List<Section>());
            list.AddRange(Products ?? new List<Product>());
            list.AddRange(ProductModels ?? new List<ProductModel>());
            return list.OrderBy(x => x.Name);
        }

        public List<string> GetProductPropertyList() => 
            JsonConvert.DeserializeObject<List<string>>(ProductProperties);

        public List<string> GetProductModelPropertyList() =>
            JsonConvert.DeserializeObject<List<string>>(ProductModelProperties);

        public override string GetImageFullLink() => $"/images/{Name}/{MainImageLink}";

        public bool IsMainSection() => ParentSection == null;

        public bool HasChildSections() => ChildSections.Count != 0;
    }
}
