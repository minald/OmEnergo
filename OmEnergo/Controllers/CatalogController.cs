using Microsoft.AspNetCore.Mvc;
using OmEnergo.Models;
using OmEnergo.Models.ViewModels;

namespace OmEnergo.Controllers
{
	public class CatalogController : Controller
	{
		private Repository Repository { get; set; }

		public CatalogController(Repository repository) => Repository = repository;

		public IActionResult Index() => View(Repository.GetMainSections());

		public IActionResult Search(string searchString) => View(new SearchViewModel(searchString,
            Repository.GetSearchedSections(searchString), Repository.GetSearchedProducts(searchString),
            Repository.GetSearchedProductModels(searchString)));
        
        public IActionResult Products(string name)
        {
            var section = Repository.GetSection(name);
            if (section != null)
            {
                ViewData["Title"] = section.Name;
                return View("Section", section);
            }

            var product = Repository.GetProduct(name);
            if (product != null)
            {
                ViewData["Title"] = product.Name;
                return View("Product", product);
            }

            var productModel = Repository.GetProductModel(name);
            ViewData["Title"] = productModel.Name;
            return View("ProductModel", productModel);
        }
    }
}
