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
            var commonObject = Repository.GetByEnglishName(name);
            ViewData["Title"] = commonObject.Name;
            return View(commonObject.GetType().Name, commonObject);
        }
    }
}
