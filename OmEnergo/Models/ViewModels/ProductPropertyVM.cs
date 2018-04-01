using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace OmEnergo.Models.ViewModels
{
    public class ProductPropertyVM
    {
        public string DisplayName { get; set; }

        public string Value { get; set; }

        public ProductPropertyVM(object model, string propertyName)
        {
            var modelType = model.GetType();
            var propertyInfo = modelType.GetProperty(propertyName);
            var displayAttribute = propertyInfo.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
            DisplayName = displayAttribute?.Name;
            Value = propertyInfo.GetValue(model)?.ToString();
        }
    }
}
