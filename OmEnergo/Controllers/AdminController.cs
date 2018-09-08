using Microsoft.AspNetCore.Mvc;
using OmEnergo.Models;

namespace OmEnergo.Controllers
{
	[AdminFilter]
    public class AdminController : Controller
    {
        private Repository Repository { get; set; }

        public AdminController(OmEnergoContext db) => Repository = new Repository(db);

        public IActionResult Sections() => View(Repository.GetMainSections());

        public IActionResult Products(string name)
        {
            ViewData["Title"] = name;
            return View(Repository.GetProducts(name));
        }

        public IActionResult ProductModels(string sectionName, string productName)
        {
            ViewData["Title"] = productName;
            return View(Repository.GetProductModels(sectionName, productName));
        }

        public IActionResult EditProductModel(int id) => View(Repository.GetCommonProductModel(id));

        [HttpPost]
        public IActionResult EditProductModel(CommonProductModel commonProductModel)
        {
            Repository.SaveCommonProductModel(commonProductModel);
            return RedirectToAction("Sections");
        }
    }
}
