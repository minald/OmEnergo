using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OmEnergo.Infrastructure.Database;

namespace OmEnergo.Controllers
{
	public class CatalogController : Controller
	{
		private readonly SectionRepository sectionRepository;
		private readonly CompoundRepository compoundRepository;
		private readonly IStringLocalizer localizer;

		public CatalogController(SectionRepository sectionRepository, 
			CompoundRepository compoundRepository, 
			IStringLocalizer localizer)
		{
			this.sectionRepository = sectionRepository;
			this.compoundRepository = compoundRepository;
			this.localizer = localizer;
		}

		public IActionResult Index()
		{
			var mainSections = sectionRepository.GetOrderedMainSections();
			ViewData["Title"] = localizer["Catalog"];
			return View("_PanelWithCards", mainSections);
		}

		public IActionResult Products(string name)
		{
			var commonObject = compoundRepository.GetObjectByEnglishName(name);
			ViewData["Title"] = commonObject.MetatagTitle ?? commonObject.Name;
			ViewData["MetatagDescription"] = commonObject.MetatagDescription ?? commonObject.Name;
			return View(commonObject);
		}
	}
}
