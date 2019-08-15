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
		private Repository Repository { get; set; }

		public HomeController(Repository repository) => Repository = repository;

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
			string correctLogin = Repository.GetConfigValue("AdminLogin");
			string correctPassword = Repository.GetConfigValue("AdminPassword");
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
			Repository.GetSearchedSections(searchString), Repository.GetSearchedProducts(searchString),
			Repository.GetSearchedProductModels(searchString)));

		public IActionResult Error() => View();
	}
}
