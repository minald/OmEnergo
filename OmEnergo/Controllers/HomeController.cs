using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OmEnergo.Models;
using OmEnergo.Models.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OmEnergo.Controllers
{
    public class HomeController : Controller
    {
        private Repository Repository { get; set; }
        public IConfiguration Configuration { get; set; }

        public HomeController(Repository repository, IConfiguration configuration)
        {
            Repository = repository;
            Configuration = configuration;
        }

        public IActionResult Index() => View();

        public IActionResult About() => View();

        public IActionResult Payment() => View();

        public IActionResult Delivery() => View();

        public IActionResult Contacts() => View();

        public IActionResult Error() => View();

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

        public IActionResult ViewPdf(string path)
        {
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, "application/pdf");
        }
    }
}
