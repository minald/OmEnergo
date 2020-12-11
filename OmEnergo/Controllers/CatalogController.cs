using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OmEnergo.Infrastructure.Database;

namespace OmEnergo.Controllers
{
	public class CatalogController : Controller
	{
		private readonly Repository repository; 
		private readonly IStringLocalizer localizer;

		public CatalogController(Repository repository, IStringLocalizer localizer)
		{
			this.repository = repository;
			this.localizer = localizer;
		}

		public IActionResult Index()
		{
			var mainSections = repository.GetOrderedMainSections();
			ViewData["Title"] = localizer["Catalog"];
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
