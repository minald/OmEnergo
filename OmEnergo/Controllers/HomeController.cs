using Microsoft.AspNetCore.Mvc;
using OmEnergo.Services;

namespace OmEnergo.Controllers
{
    public class HomeController : Controller
    {
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
		public IActionResult Feedback(string name, string text, string email = "", string phone = "")
		{
			return View();
		}

        public IActionResult Error() => View();
    }
}
