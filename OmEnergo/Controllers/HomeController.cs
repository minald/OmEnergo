using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OmEnergo.Infrastructure;
using OmEnergo.Infrastructure.Database;
using OmEnergo.Models.ViewModels;
using System;
using System.Threading.Tasks;

namespace OmEnergo.Controllers
{
	public class HomeController : Controller
	{
		private readonly Repository repository;

		public HomeController(Repository repository) => this.repository = repository;

		public IActionResult About() => View();

		public IActionResult PaymentAndDelivery() => View();

		public IActionResult Contacts() => View();

		[HttpPost]
		public IActionResult Contacts([FromServices] EmailSender emailSender, 
			string name, string text, string email, string phoneNumber = "")
		{
			if (!String.IsNullOrEmpty(name) || !String.IsNullOrEmpty(text) || !String.IsNullOrEmpty(email))
			{
				Task.Factory.StartNew(() => emailSender.SendEmail(name, phoneNumber, email, text));
			}

			return RedirectToAction("Index", "Catalog");
		}

		public IActionResult Login() => View();

		[HttpPost]
		public IActionResult Login(string login, string password)
		{
			var correctLogin = repository.GetConfigValue("AdminLogin");
			var correctPassword = repository.GetConfigValue("AdminPassword");
			if (login == correctLogin && password == correctPassword)
			{
				HttpContext.Session.SetString("isLogin", "true");
				return RedirectToAction("Index", "Admin");
			}
			else
			{
				TempData["message"] = "Введённые данные неверны";
				return RedirectToAction(nameof(Login));
			}
		}

		public IActionResult Search(string searchString) => View(new SearchViewModel(searchString,
			repository.GetSearchedSections(searchString), repository.GetSearchedProducts(searchString),
			repository.GetSearchedProductModels(searchString)));

		public IActionResult Error() => View();
	}
}
