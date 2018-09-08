using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OmEnergo.Models;
using System;
using System.Threading.Tasks;

namespace OmEnergo.Controllers
{
    public class HomeController : Controller
    {
		public IConfiguration Configuration { get; set; }

		public HomeController(IConfiguration configuration) => Configuration = configuration;

		public IActionResult Index() => View();

        public IActionResult About() => View();

        public IActionResult Payment() => View();

        public IActionResult Delivery() => View();

        public IActionResult Contacts() => View();

		[HttpPost]
		public IActionResult Contacts(string name, string text, string email, string phoneNumber = "")
		{
			if (!String.IsNullOrEmpty(name) || !String.IsNullOrEmpty(text) || !String.IsNullOrEmpty(email))
			{
			    Task.Factory.StartNew(() => new EmailSender(Configuration).SendEmail(name, text, email, phoneNumber));
			}

			return RedirectToAction("Index", "Catalog");
		}

		public IActionResult Login() => View();

		[HttpPost]
		public IActionResult Login(string login, string password)
		{
			if (login == "Admin" && password == "123")
			{
				HttpContext.Session.SetString("isLogin", "true");
				return RedirectToAction("Sections", "Admin");
			}
			else
			{
				TempData["LoginError"] = "Введённые данные неверны";
			}
			return RedirectToAction(nameof(Login));
		}

        public IActionResult Error() => View();
    }
}
