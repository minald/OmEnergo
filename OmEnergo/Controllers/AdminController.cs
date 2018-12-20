﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OmEnergo.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Controllers
{
    //[AdminAuthorizationFilter]
    public class AdminController : Controller
    {
        private Repository Repository { get; set; }
        private IHostingEnvironment AppEnvironment { get; set; }

        public AdminController(OmEnergoContext db, IHostingEnvironment appEnvironment)
        {
            Repository = new Repository(db);
            AppEnvironment = appEnvironment;
        }

        #region Sections

        public IActionResult Sections() => View(Repository.GetMainSections().OrderBy(x => x.SequenceNumber));

        public IActionResult Section(int id) => View(Repository.GetSection(id));

        public IActionResult CreateSection(int parentId) => View("CreateOrEditSection",
            new Section() { ParentSection = Repository.Get<Section>(parentId) });

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

            section.SetEnglishNameIfEmpty();
            Repository.Update(section);
            TempData["message"] = $"Секция {section.Name} создана";
            return section.IsMainSection() ? RedirectToAction(nameof(Sections))
                : RedirectToAction(nameof(Section), new { id = section.ParentSection.Id });
        }

        public IActionResult EditSection(int id) => View("CreateOrEditSection", Repository.Get<Section>(id));

        [HttpPost]
        public IActionResult EditSection(Section section, int? parentSectionId)
        {
            if (parentSectionId != null)
            {
                section.ParentSection = Repository.GetSection(parentSectionId.Value);
            }

            section.SetEnglishNameIfEmpty();
            Repository.UpdateSectionAndSynchronizeProperties(section);
            TempData["message"] = $"Секция {section.Name} изменена";
            return section.IsMainSection() ? RedirectToAction(nameof(Sections))
                : RedirectToAction(nameof(Section), new { id = section.ParentSection.Id });
        }

        [HttpPost]
        public IActionResult DeleteSection(int id)
        {
            var section = Repository.GetSection(id);
            Repository.Delete<Section>(id);
            TempData["message"] = $"Секция {section.Name} удалена";
            return Redirect(Request.Headers["Referer"].ToString());
        }

        #endregion

        #region Products

        public IActionResult Product(int id) => View(Repository.GetProduct(id));

        public IActionResult CreateProduct(int sectionId) => 
            View("CreateOrEditProduct", new Product(Repository.Get<Section>(sectionId)));

        [HttpPost]
        public IActionResult CreateProduct(Product product, int? sectionId, params string[] values)
        {
            product.Section = Repository.GetSection(sectionId.Value);
            product.SequenceNumber = product.Section.GetNestedObjects().Count() + 1;
            product.UpdatePropertyValues(values);
            product.SetEnglishNameIfEmpty();
            Repository.Update(product);
            TempData["message"] = $"Продукт {product.Name} создан";
            return RedirectToAction(nameof(Section), new { id = product.Section.Id });
        }

        public IActionResult EditProduct(int id) => View("CreateOrEditProduct", Repository.GetProduct(id));

        [HttpPost]
        public IActionResult EditProduct(Product product, int? sectionId, params string[] values)
        {
            product.Section = Repository.Get<Section>(sectionId);
            product.UpdatePropertyValues(values);
            product.SetEnglishNameIfEmpty();
            Repository.Update(product);
            TempData["message"] = $"Продукт {product.Name} изменён";
            return RedirectToAction(nameof(Section), new { id = product.Section.Id });
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            Repository.Delete<Product>(id);
            TempData["message"] = $"Продукт удалён";
            return Redirect(Request.Headers["Referer"].ToString());
        }

        #endregion

        #region ProductModels

        public IActionResult CreateProductModel(int sectionId, int productId) => View("CreateOrEditProductModel",
            new ProductModel(Repository.Get<Section>(sectionId), Repository.GetProduct(productId)));

        [HttpPost]
        public IActionResult CreateProductModel(ProductModel productModel, int? sectionId, int? productId, params string[] values)
        {
            productModel.Section = Repository.GetSection(sectionId.GetValueOrDefault());
            productModel.Product = Repository.GetProduct(productId.GetValueOrDefault());
            productModel.SequenceNumber =
                (sectionId == null ? productModel.Product.Models : productModel.Section.GetNestedObjects()).Count() + 1;
            productModel.UpdatePropertyValues(values);
            productModel.SetEnglishNameIfEmpty();
            Repository.Update(productModel);
            TempData["message"] = $"Модель {productModel.Name} создана";
            return sectionId == null ? RedirectToAction(nameof(Product), new { id = productModel.Product.Id })
                : RedirectToAction(nameof(Section), new { id = productModel.Section.Id });
        }

        public IActionResult EditProductModel(int id) => 
            View("CreateOrEditProductModel", Repository.GetProductModel(id));

        [HttpPost]
        public IActionResult EditProductModel(ProductModel productModel, int? sectionId, int? productId, params string[] values)
        {
            productModel.Section = Repository.Get<Section>(sectionId);
            productModel.Product = Repository.Get<Product>(productId);
            productModel.UpdatePropertyValues(values);
            productModel.SetEnglishNameIfEmpty();
            Repository.Update(productModel);
            TempData["message"] = $"Модель {productModel.Name} изменена";
            return sectionId == null ? RedirectToAction(nameof(Product), new { id = productModel.Product.Id })
                : RedirectToAction(nameof(Section), new { id = productModel.Section.Id });
        }

        [HttpPost]
        public IActionResult DeleteProductModel(int id)
        {
            Repository.Delete<ProductModel>(id);
            TempData["message"] = $"Модель удалена";
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public async Task<IActionResult> UploadProductModelPhoto(int id, IFormFile uploadedFile)
        {
            var productModel = Repository.GetProductModel(id);
            if (uploadedFile != null)
            {
                string path = AppEnvironment.WebRootPath + productModel.GetImageFullLink();
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
            }

            return productModel.Section == null ? RedirectToAction(nameof(Product), new { id = productModel.Product.Id })
                : RedirectToAction(nameof(Section), new { id = productModel.Section.Id });
        }

        #endregion
    }
}
