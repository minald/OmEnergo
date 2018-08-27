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

        public IActionResult Contact() => View();

		[HttpPost]
		public IActionResult Feedback(string name, string text, string email, string phoneNumber = "")
		{
			if (!String.IsNullOrEmpty(name) || !String.IsNullOrEmpty(text) || !String.IsNullOrEmpty(email))
			{
			    Task.Factory.StartNew(() => new EmailSender(Configuration).SendEmail(name, text, email, phoneNumber));
			}

			return RedirectToAction("Index", "Catalog");
		}

        public IActionResult Error() => View();
    }
}
