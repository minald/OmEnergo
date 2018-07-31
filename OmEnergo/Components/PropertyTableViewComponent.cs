using Microsoft.AspNetCore.Mvc;
using OmEnergo.Models;
using System.Collections.Generic;
using System.Linq;

namespace OmEnergo.Components
{
    public class PropertyTableViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(object model, IEnumerable<string> propertyNames) => 
            View(propertyNames.Select(x => new ProductProperty(model, x)));
    }
}
