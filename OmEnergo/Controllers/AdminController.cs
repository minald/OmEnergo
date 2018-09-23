using Microsoft.AspNetCore.Mvc;
using OmEnergo.Models;
using System.Linq;

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
        public IActionResult CreateOrEditSection(Section section, int? parentSectionId)
        {
            if (parentSectionId != null)
            {
                section.ParentSection = Repository.Get<Section>(parentSectionId);
            }

            Repository.Update(section);

            var products = Repository.GetProducts(section.Id).ToList();
            products.ForEach(x => x.UpdateProperties(section.ProductProperties));
            Repository.UpdateRange(products);

            var productModels = Repository.GetProductModels(section.Id).ToList();
            productModels.ForEach(x => x.UpdateProperties(section.ProductModelProperties));
            Repository.UpdateRange(productModels);

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
            ViewData["Title"] = sectionName;
            ViewBag.SectionId = Repository.GetSection(sectionName).Id;
            return View(Repository.GetProducts(sectionName));
        }

        public IActionResult CreateProduct(int sectionId) => 
            View("CreateOrEditProduct", new CommonProduct() { Section = Repository.Get<Section>(sectionId) });

        public IActionResult EditProduct(int id) => View("CreateOrEditProduct", Repository.Get<CommonProduct>(id));

        [HttpPost]
        public IActionResult CreateOrEditProduct(CommonProduct commonProduct, int? sectionId)
        {
            if (commonProduct.Section == null)
            {
                commonProduct.Section = Repository.Get<Section>(sectionId);
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
            ViewData["Title"] = productName;
            ViewBag.ProductId = Repository.GetProduct(sectionName, productName).Id;
            return View(Repository.GetProductModels(sectionName, productName));
        }

        public IActionResult CreateProductModel(int productId) => View("CreateOrEditProductModel",
            new CommonProductModel() { CommonProduct = Repository.Get<CommonProduct>(productId) });

        public IActionResult EditProductModel(int id) => View("CreateOrEditProductModel", Repository.Get<CommonProductModel>(id));

        [HttpPost]
        public IActionResult CreateOrEditProductModel(CommonProductModel commonProductModel, int? productId)
        {
            if(commonProductModel.CommonProduct == null)
            {
                commonProductModel.CommonProduct = Repository.Get<CommonProduct>(productId);
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
