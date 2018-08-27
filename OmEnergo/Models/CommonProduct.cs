using Newtonsoft.Json;
using System.Collections.Generic;

namespace OmEnergo.Models
{
    public class CommonProduct
    {
        public int Id { get; set; }

        public Section Section { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Properties { get; set; }

        public string MainImageLink { get; set; }

        public List<CommonProductModel> Models { get; set; }

        public IEnumerable<ProductProperty> GetProperties()
        {
            var properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(Properties);
            foreach(var pair in properties)
            {
                yield return new ProductProperty(pair.Key, pair.Value);
            }
        }

        public string GetImageFullLink() => $"/images/{Section.EnglishName}/{Name.Replace('"', '\'')}/{MainImageLink}";
    }
}
