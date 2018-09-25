using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class Section : CommonObject
    {
        public Section ParentSection { get; set; }

        public List<Section> ChildSections { get; set; }

        [Display(Name = "Ссылка на фото")]
        public string MainImageLink { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Свойства продукта")]
        public string ProductProperties { get; set; }

        [Display(Name = "Свойства модели продукта")]
        public string ProductModelProperties { get; set; }

        public Section()
        {
            string emptyArrayInJson = "[]";
            ProductProperties = emptyArrayInJson;
            ProductModelProperties = emptyArrayInJson;
        }

        public List<string> GetProductPropertiesList() => 
            JsonConvert.DeserializeObject<List<string>>(ProductProperties);

        public List<string> GetProductModelPropertiesList() =>
            JsonConvert.DeserializeObject<List<string>>(ProductModelProperties);

        public override string GetImageFullLink() => $"/images/{Name}/{MainImageLink}";

        public bool IsMainSection() => ParentSection == null;

        public bool HasChildSections() => ChildSections.Count != 0;
    }
}
