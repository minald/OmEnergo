using Microsoft.AspNetCore.Mvc;
using OmEnergo.Services;
using System.Threading.Tasks;

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

		public IActionResult Feedback() => PartialView();

		[HttpPost]
		public async Task<IActionResult> Feedback(string name, string text, string email = "", string phoneNumber = "")
		{
			await emailService.SendEmail(name, text, email, phoneNumber);
			return RedirectToAction("Index", "Catalog");
		}

        public IActionResult Error() => View();
    }
}
