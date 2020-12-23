using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OmEnergo.Infrastructure.Database;
using OmEnergo.Models;
using OmEnergo.Models.ViewModels;
using OmEnergo.Resources;
using OmEnergo.Services;
using System;
using System.Threading.Tasks;

namespace OmEnergo.Controllers
{
	public class HomeController : Controller
	{
		private readonly SectionRepository sectionRepository;
		private readonly ProductRepository productRepository;
		private readonly ProductModelRepository productModelRepository;
		private readonly ConfigKeyRepository configKeyRepository;
		private readonly IStringLocalizer localizer;
		private readonly SignInManager<IdentityUser> signInManager;

		public HomeController(SectionRepository sectionRepository, 
			ProductRepository productRepository, 
			ProductModelRepository productModelRepository, 
			ConfigKeyRepository configKeyRepository, 
			IStringLocalizer localizer,
			SignInManager<IdentityUser> signInManager)
		{
			this.sectionRepository = sectionRepository;
			this.productRepository = productRepository;
			this.productModelRepository = productModelRepository;
			this.configKeyRepository = configKeyRepository;
			this.localizer = localizer;
			this.signInManager = signInManager;
		}

		public IActionResult About() => View();

		public IActionResult PaymentAndDelivery() => View();

		public IActionResult Contacts() => View();

		[HttpPost]
		public IActionResult Contacts([FromServices] EmailSenderService emailSenderService, 
			string name, string text, string email, string phoneNumber = "")
		{
			if (!String.IsNullOrEmpty(name) || !String.IsNullOrEmpty(text) || !String.IsNullOrEmpty(email))
			{
				Task.Factory.StartNew(() => emailSenderService.SendEmail(name, phoneNumber, email, text));
			}

			return RedirectToAction("Index", "Catalog");
		}

		public IActionResult Login() => View();

		[HttpPost]
		public IActionResult Login(string login, string password)
		{
			var result = signInManager.PasswordSignInAsync(login, password, false, false).Result;
			if (result.Succeeded)
			{
				return RedirectToAction("Index", "Admin");
			}
			else
			{
				TempData["message"] = localizer[nameof(SharedResource.TheEnteredDataIsIncorrect)].Value;
				return RedirectToAction(nameof(Login));
			}
		}

		public async Task<IActionResult> Search(string searchString) => View(new SearchViewModel(searchString,
			await sectionRepository.GetSearchedItemsAsync<Section>(searchString), 
			await productRepository.GetSearchedItemsAsync<Product>(searchString),
			await productModelRepository.GetSearchedItemsAsync<ProductModel>(searchString)));

		public IActionResult Error() => View();
	}
}
