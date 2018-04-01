using Microsoft.AspNetCore.Mvc;
using OmEnergo.Models;
using OmEnergo.Models.ViewModels;

namespace OmEnergo.Components
{
    public class ProductProperty : ViewComponent
    {
        public IViewComponentResult Invoke(Stabilizer model, string value)
        {
            var productProperty = new ProductPropertyVM(model, value);
            return View(productProperty);
        }
    }
}
