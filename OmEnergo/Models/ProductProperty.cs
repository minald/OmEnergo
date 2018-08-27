using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace OmEnergo.Models
{
    public class ProductProperty
    {
        public string DisplayName { get; set; }

        public string Value { get; set; }

        public ProductProperty(string displayName, string value)
        {
            DisplayName = displayName;
            Value = value;
        }
    }
}
