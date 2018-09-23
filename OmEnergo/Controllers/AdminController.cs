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

        public IActionResult CreateSection(string parentName) => View("CreateOrEditSection", 
            new Section() { ParentSection = Repository.GetSection(parentName) });

        public IActionResult EditSection(int id) => View("CreateOrEditSection", Repository.Get<Section>(id));

        [HttpPost]
        public IActionResult CreateOrEditSection(Section section, string parentSectionName)
        {
            if (parentSectionName != null)
            {
                section.ParentSection = Repository.GetSection(parentSectionName);
            }

            Repository.Update(section);
            return RedirectToAction("Sections");
        }

        [HttpPost]
        public IActionResult DeleteSection(int id)
        {
            Repository.Delete<Section>(id);
            return RedirectToAction("Sections");
        }

        public IActionResult Products(string sectionName)
        {
            HttpContext.Session.SetString("CurrentSectionName", sectionName);
            ViewData["Title"] = sectionName;
            return View(Repository.GetProducts(sectionName));
        }

        public IActionResult CreateProduct() => View("CreateOrEditProduct");

        public IActionResult EditProduct(int id) => View("CreateOrEditProduct", Repository.Get<CommonProduct>(id));

        [HttpPost]
        public IActionResult CreateOrEditProduct(CommonProduct commonProduct)
        {
            if (commonProduct.Section == null)
            {
                commonProduct.Section = Repository.GetSection(
                    HttpContext.Session.GetString("CurrentSectionName"));
            }

            Repository.Update(commonProduct);
            return RedirectToAction("Sections");
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            Repository.Delete<CommonProduct>(id);
            return RedirectToAction("Sections");
        }

        public IActionResult ProductModels(string sectionName, string productName)
        {
            HttpContext.Session.SetString("CurrentSectionName", sectionName);
            HttpContext.Session.SetString("CurrentProductName", productName);
            ViewData["Title"] = productName;
            return View(Repository.GetProductModels(sectionName, productName));
        }

        public IActionResult CreateProductModel() => View("CreateOrEditProductModel");

        public IActionResult EditProductModel(int id) => View("CreateOrEditProductModel", Repository.Get<CommonProductModel>(id));

        [HttpPost]
        public IActionResult CreateOrEditProductModel(CommonProductModel commonProductModel)
        {
            if(commonProductModel.CommonProduct == null)
            {
                commonProductModel.CommonProduct = Repository.GetCommonProduct(
                    HttpContext.Session.GetString("CurrentSectionName"), HttpContext.Session.GetString("CurrentProductName"));
            }

            Repository.Update(commonProductModel);
            return RedirectToAction("Sections");
        }

        [HttpPost]
        public IActionResult DeleteProductModel(int id)
        {
            Repository.Delete<CommonProductModel>(id);
            return RedirectToAction("Sections");
        }
    }
}
