using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OmEnergo.Resources;
using OmEnergo.Services;
using System.Threading.Tasks;

namespace OmEnergo.Controllers
{
	public class CatalogController : Controller
	{
		private readonly RepositoryService repositoryService;
		private readonly IStringLocalizer localizer;

		public CatalogController(RepositoryService repositoryService, IStringLocalizer localizer)
		{
			this.repositoryService = repositoryService;
			this.localizer = localizer;
		}

		public async Task<IActionResult> Index()
		{
			var mainSections = await repositoryService.GetOrderedMainSectionsAsync();
			ViewData["Title"] = localizer[nameof(SharedResource.Catalog)];
			return View("_PanelWithCards", mainSections);
		}

		public async Task<IActionResult> Products(string name)
		{
			var commonObject = await repositoryService.GetObjectByEnglishNameAsync(name);
			ViewData["Title"] = commonObject.MetatagTitle ?? commonObject.Name;
			ViewData["MetatagDescription"] = commonObject.MetatagDescription ?? commonObject.Name;
			return View(commonObject);
		}
	}
}
