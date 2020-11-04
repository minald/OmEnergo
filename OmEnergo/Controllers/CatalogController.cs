using Microsoft.AspNetCore.Mvc;
using OmEnergo.Infrastructure.Database;

namespace OmEnergo.Controllers
{
	public class CatalogController : Controller
	{
		private readonly Repository repository;

		public CatalogController(Repository repository) => this.repository = repository;

		public IActionResult Index()
		{
			var mainSections = repository.GetOrderedMainSections();
			ViewData["Title"] = "Каталог";
			return View("_PanelWithCards", mainSections);
		}

		public IActionResult Products(string name)
		{
			var commonObject = repository.GetObjectByEnglishName(name);
			ViewData["Title"] = commonObject.MetatagTitle ?? commonObject.Name;
			ViewData["MetatagDescription"] = commonObject.MetatagDescription ?? commonObject.Name;
			return View(commonObject);
		}
	}
}
