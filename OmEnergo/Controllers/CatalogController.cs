using Microsoft.AspNetCore.Mvc;
using OmEnergo.Infrastructure.Database;
using System.Linq;

namespace OmEnergo.Controllers
{
	public class CatalogController : Controller
	{
		private Repository Repository { get; set; }

		public CatalogController(Repository repository) => Repository = repository;

        public IActionResult Index()
        {
            var mainSections = Repository.GetOrderedMainSections();
            ViewData["Title"] = "Каталог";
            return View("_PanelWithCards", mainSections);
        }

        public IActionResult Products(string name)
        {
            var commonObject = Repository.GetObjectByEnglishName(name);
            ViewData["Title"] = commonObject.Name;
            return View(commonObject);
        }
    }
}
