using Microsoft.AspNetCore.Mvc;
using OmEnergo.Services;
using System.Threading.Tasks;

namespace OmEnergo.Controllers
{
    public class HomeController : Controller
    {
		public IActionResult Index() => View();

        public IActionResult About() => View();

        public IActionResult Payment() => View();

        public IActionResult Delivery() => View();

        public IActionResult Contact() => View();

		public IActionResult Feedback() => PartialView();

		[HttpPost]
		public async Task<IActionResult> Feedback(string name, string text, string email = "", string phoneNumber = "")
		{
			await EmailService.SendEmail(name, text, email, phoneNumber);
			return RedirectToAction("Index", "Catalog");
		}

        public IActionResult Error() => View();
    }
}
