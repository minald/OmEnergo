using Microsoft.AspNetCore.Mvc;
using OmEnergo.Models;
using System.Collections.Generic;

namespace OmEnergo.Components
{
    public class PropertyTableViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<ProductProperty> properties) => View(properties);
    }
}
