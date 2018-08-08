using Microsoft.AspNetCore.Mvc;
using OmEnergo.Services;

namespace OmEnergo.Controllers
{
    public class HomeController : Controller
    {
		EmailService emailService { get; set; }

		public HomeController()
		{
			emailService = new EmailService();
		}

		public IActionResult Index()
		{
			return View();
		}

        public IActionResult About() => View();

        public IActionResult Payment() => View();

        public IActionResult Delivery() => View();

        public IActionResult Contact() => View();

		public IActionResult Feedback() => View();

		[HttpPost]
		public IActionResult Feedback(string name, string text, string email = "", string phoneNumber = "")
		{
			emailService.SendEmail(name, text, email, phoneNumber);
			return View();
		}

        public IActionResult Error() => View();
    }
}
