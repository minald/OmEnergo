using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class CommonProduct
    {
        public int Id { get; set; }

        public Section Section { get; set; }

        public List<CommonProductModel> Models { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Ссылка на фото")]
        public string MainImageLink { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Свойства")]
        public string Properties { get; set; }

        public IEnumerable<ProductProperty> GetProperties()
        {
            var properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(Properties);
            foreach(var pair in properties)
            {
                yield return new ProductProperty(pair.Key, pair.Value);
            }
        }

        public string GetImageFullLink() => $"/images/{Section.Name}/{Name.Replace('"', '\'')}/{MainImageLink}";
    }
}
