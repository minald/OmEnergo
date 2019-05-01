using Microsoft.AspNetCore.Mvc;
using OmEnergo.Models;

namespace OmEnergo.Controllers
{
	public class CatalogController : Controller
	{
		private Repository Repository { get; set; }

		public CatalogController(Repository repository) => Repository = repository;

		public IActionResult Index() => View(Repository.GetMainSections());
        
        public IActionResult Products(string name)
        {
            var commonObject = Repository.GetObjectByEnglishName(name);
            ViewData["Title"] = commonObject.Name;
            return View(commonObject.GetType().Name, commonObject);
        }
    }
}
