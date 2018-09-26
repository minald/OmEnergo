﻿using Microsoft.AspNetCore.Mvc;
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
        public IActionResult CreateOrEditSection(Section section, int? parentSectionId)
        {
            if (parentSectionId != null)
            {
                section.ParentSection = Repository.Get<Section>(parentSectionId);
            }

            Repository.UpdateSectionAndSynchronizeProperties(section);
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
            View("CreateOrEditProduct", new Product() { Section = Repository.Get<Section>(sectionId) });

        public IActionResult EditProduct(int id) => View("CreateOrEditProduct", Repository.Get<Product>(id));

        [HttpPost]
        public IActionResult CreateOrEditProduct(Product product, int? sectionId)
        {
            if (product.Section == null)
            {
                product.Section = Repository.Get<Section>(sectionId);
            }

            Repository.Update(product);
            return RedirectToAction("Sections");
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            Repository.Delete<Product>(id);
            return RedirectToAction("Sections");
        }

        public IActionResult ProductModels(string sectionName, string productName)
        {
            ViewData["Title"] = productName;
            ViewBag.ProductId = Repository.GetProduct(sectionName, productName).Id;
            return View(Repository.GetProductModels(sectionName, productName));
        }

        public IActionResult CreateProductModel(int productId) => View("CreateOrEditProductModel",
            new ProductModel() { Product = Repository.Get<Product>(productId) });

        public IActionResult EditProductModel(int id) => View("CreateOrEditProductModel", Repository.Get<ProductModel>(id));

        [HttpPost]
        public IActionResult CreateOrEditProductModel(ProductModel productModel, int? productId)
        {
            if(productModel.Product == null)
            {
                productModel.Product = Repository.Get<Product>(productId);
            }

            Repository.Update(productModel);
            return RedirectToAction("Sections");
        }

        [HttpPost]
        public IActionResult DeleteProductModel(int id)
        {
            Repository.Delete<ProductModel>(id);
            return RedirectToAction("Sections");
        }
    }
}
