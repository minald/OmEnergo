using Microsoft.AspNetCore.Http;
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

        public IActionResult EditSection(int id) => View(Repository.GetSection(id));

        [HttpPost]
        public IActionResult EditSection(Section section)
        {
            Repository.SaveSection(section);
            return RedirectToAction("Sections");
        }

        public IActionResult Products(string sectionName)
        {
            HttpContext.Session.SetString("CurrentSectionName", sectionName);
            ViewData["Title"] = sectionName;
            return View(Repository.GetProducts(sectionName));
        }

        public IActionResult EditProduct(int id) => View(Repository.GetCommonProduct(id));

        [HttpPost]
        public IActionResult EditProduct(CommonProduct commonProduct)
        {
            Repository.SaveCommonProduct(commonProduct);
            return RedirectToAction("Sections");
        }

        public IActionResult ProductModels(string sectionName, string productName)
        {
            HttpContext.Session.SetString("CurrentSectionName", sectionName);
            HttpContext.Session.SetString("CurrentProductName", productName);
            ViewData["Title"] = productName;
            return View(Repository.GetProductModels(sectionName, productName));
        }

        public IActionResult CreateProductModel() => View("EditProductModel");

        public IActionResult EditProductModel(int id) => View(Repository.GetCommonProductModel(id));

        [HttpPost]
        public IActionResult EditProductModel(CommonProductModel commonProductModel)
        {
            if(commonProductModel.CommonProduct == null)
            {
                commonProductModel.CommonProduct = Repository.GetCommonProduct(
                    HttpContext.Session.GetString("CurrentSectionName"), HttpContext.Session.GetString("CurrentProductName"));
            }

            Repository.SaveCommonProductModel(commonProductModel);
            return RedirectToAction("Sections");
        }
    }
}
