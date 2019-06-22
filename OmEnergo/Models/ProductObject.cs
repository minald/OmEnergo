using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OmEnergo.Models
{
    public abstract class ProductObject : CommonObject
    {
        [Display(Name = "Свойства")]
        public string Properties { get; set; } //Json object in DB

        public IDictionary<string, string> GetProperties() 
            => JsonConvert.DeserializeObject<Dictionary<string, string>>(Properties);

        public IDictionary<string, string> GetPropertiesWithValues()
            => GetProperties().Where(p => !String.IsNullOrWhiteSpace(p.Value)).ToDictionary(x => x.Key, x => x.Value);

        public string GetPropertyNames() => String.Join(';', GetProperties().Select(x => x.Key));

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

        public void UpdatePropertyValues(params string[] propertyValues)
        {
            var result = new Dictionary<string, string>();
            var propertyKeys = JsonConvert.DeserializeObject<Dictionary<string, string>>(Properties ?? "{}").Keys;
            for (int i = 0; i < propertyKeys.Count; i++)
            {
                result.Add(propertyKeys.ElementAt(i), propertyValues[i] ?? "");
            }

            Properties = JsonConvert.SerializeObject(result);
        }
    }
}
