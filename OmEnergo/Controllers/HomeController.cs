using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OmEnergo.Resources;
using OmEnergo.Services;
using System.Threading.Tasks;

namespace OmEnergo.Controllers
{
	public class HomeController : Controller
	{
		private readonly RepositoryService repositoryService;
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly IStringLocalizer localizer;

		public HomeController(RepositoryService repositoryService,
			SignInManager<IdentityUser> signInManager,
			IStringLocalizer localizer)
		{
			this.repositoryService = repositoryService;
			this.signInManager = signInManager;
			this.localizer = localizer;
		}

		public IActionResult About() => View();

		public IActionResult PaymentAndDelivery() => View();

		public IActionResult Contacts() => View();

		[HttpPost]
		public IActionResult Contacts([FromServices] EmailSenderService emailSenderService, 
			string name, string text, string email, string phoneNumber = "")
		{
			emailSenderService.SendEmail(name, phoneNumber, email, text);
			return RedirectToAction(nameof(CatalogController.Index), "Catalog");
		}

		public IActionResult Login() => View();

		[HttpPost]
		public IActionResult Login(string login, string password)
		{
			var result = signInManager.PasswordSignInAsync(login, password, false, false).Result;
			if (result.Succeeded)
			{
				return RedirectToAction(nameof(AdminController.Index), "Admin");
			}
			else
			{
				TempData["message"] = localizer[nameof(SharedResource.TheEnteredDataIsIncorrect)].Value;
				return RedirectToAction(nameof(Login));
			}
		}

		public async Task<IActionResult> Search(string searchString) => 
			View(await repositoryService.CreateSearchViewModelAsync(searchString));

		public IActionResult Error() => View();
	}
}
