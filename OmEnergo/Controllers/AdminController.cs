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

        public IActionResult Sections() => View(Repository.GetMainSections().OrderBy(x => x.SequenceNumber));

        public IActionResult CreateSection(int parentId) => View("CreateOrEditSection",
            new Section() { ParentSection = Repository.Get<Section>(parentId) });

        public IActionResult EditSection(int id) => View("CreateOrEditSection", Repository.Get<Section>(id));

        [HttpPost]
        public IActionResult CreateSection(Section section, int? parentSectionId)
        {
            if (parentSectionId != null)
            {
                section.ParentSection = Repository.GetSection(parentSectionId.Value);
                section.SequenceNumber = section.ParentSection.GetNestedObjects().Count() + 1;
            }
            else
            {
                section.SequenceNumber = Repository.GetMainSections().Count() + 1;
            }

            Repository.Update(section);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public IActionResult EditSection(Section section, int? parentSectionId)
        {
            if (parentSectionId != null)
            {
                section.ParentSection = Repository.GetSection(parentSectionId.Value);
            }

            Repository.UpdateSectionAndSynchronizeProperties(section);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public IActionResult DeleteSection(int id)
        {
            var section = Repository.GetSection(id);
            Repository.Delete<Section>(id);
            Repository.UpdateSectionsSequenceNumbers(section.ParentSection?.Id, section.SequenceNumber);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult Section(string sectionName)
        {
            ViewData["Title"] = sectionName;
            return View(Repository.GetSection(sectionName));
        }

        public IActionResult CreateProduct(int sectionId) => 
            View("CreateOrEditProduct", new Product(Repository.Get<Section>(sectionId)));

        public IActionResult EditProduct(int id) => View("CreateOrEditProduct", Repository.Get<Product>(id));

        [HttpPost]
        public IActionResult CreateProduct(Product product, int? sectionId, params string[] values)
        {
            product.Section = Repository.GetSection(sectionId.Value);
            product.SequenceNumber = product.Section.GetNestedObjects().Count() + 1;
            product.UpdatePropertyValues(values);
            Repository.Update(product);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public IActionResult EditProduct(Product product, int? sectionId, params string[] values)
        {
            product.Section = Repository.Get<Section>(sectionId);
            product.UpdatePropertyValues(values);
            Repository.Update(product);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            Repository.Delete<Product>(id);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult ProductModels(string sectionName, string productName)
        {
            ViewData["Title"] = productName;
            ViewBag.ProductId = Repository.GetProduct(sectionName, productName).Id;
            return View(Repository.GetProductModels(sectionName, productName));
        }

        public IActionResult CreateProductModel(int sectionId, int productId) => View("CreateOrEditProductModel",
            new ProductModel(Repository.Get<Section>(sectionId), Repository.GetProduct(productId)));

        public IActionResult EditProductModel(int id) => 
            View("CreateOrEditProductModel", Repository.GetProductModelById(id));

        [HttpPost]
        public IActionResult CreateProductModel(ProductModel productModel, int? sectionId, int? productId, params string[] values)
        {
            productModel.Section = Repository.GetSection(sectionId.GetValueOrDefault());
            productModel.Product = Repository.GetProduct(productId.GetValueOrDefault());
            productModel.SequenceNumber = 
                (sectionId == null ? productModel.Product.Models : productModel.Section.GetNestedObjects()).Count() + 1;
            productModel.UpdatePropertyValues(values);
            Repository.Update(productModel);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public IActionResult EditProductModel(ProductModel productModel, int? sectionId, int? productId, params string[] values)
        {
            productModel.Section = Repository.Get<Section>(sectionId);
            productModel.Product = Repository.Get<Product>(productId);
            productModel.UpdatePropertyValues(values);
            Repository.Update(productModel);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public IActionResult DeleteProductModel(int id)
        {
            Repository.Delete<ProductModel>(id);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
