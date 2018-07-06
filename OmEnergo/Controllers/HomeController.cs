using Microsoft.AspNetCore.Mvc;

namespace OmEnergo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult About() => View();

        public IActionResult Payment() => View();

        public IActionResult Delivery() => View();

        public IActionResult Contact() => View();

        public IActionResult Error() => View();
    }
}
