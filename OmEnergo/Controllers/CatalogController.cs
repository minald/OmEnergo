using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OmEnergo.Infrastructure.Database;
using System.Threading.Tasks;

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

		public async Task<IActionResult> Index()
		{
			var mainSections = await sectionRepository.GetOrderedMainSectionsAsync();
			ViewData["Title"] = localizer["Catalog"];
			return View("_PanelWithCards", mainSections);
		}

		public async Task<IActionResult> Products(string name)
		{
			var commonObject = await compoundRepository.GetObjectByEnglishNameAsync(name);
			ViewData["Title"] = commonObject.MetatagTitle ?? commonObject.Name;
			ViewData["MetatagDescription"] = commonObject.MetatagDescription ?? commonObject.Name;
			return View(commonObject);
		}
	}
}
