using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OmEnergo.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Controllers
{
    //[AdminAuthorization]
    public class AdminController : Controller
    {
        private Repository Repository { get; set; }
        private IHostingEnvironment HostingEnvironment { get; set; }

        public AdminController(Repository repository, IHostingEnvironment hostingEnvironment)
        {
            Repository = repository;
            HostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index() => View();

        public IActionResult CreateBackup()
        {
            var databaseBackuper = HttpContext.RequestServices.GetService(typeof(ExcelReportBuilder)) as ExcelReportBuilder;
            string backupPath = $@"D:\{backupName}"; //HostingEnvironment.ContentRootPath + $@"\Database\{backupName}";
            string currentDatetime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string backupName = $@"OmEnergoDB_{currentDatetime}.xlsx";
            databaseBackuper.CreateDatabaseBackup(backupPath);
            TempData["message"] = $"Бэкап базы успешно сохранён в {backupName}";
            return View(nameof(Index));
        }

        public IActionResult GetPricesReport()
        {
            var databaseBackuper = HttpContext.RequestServices.GetService(typeof(ExcelReportBuilder)) as ExcelReportBuilder;
            string currentDatetime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string backupName = $@"OmEnergoPrices_{currentDatetime}.xlsx";
            string backupPath = $@"D:\{backupName}"; //HostingEnvironment.ContentRootPath + $@"\Database\{backupName}";
            databaseBackuper.CreatePricesReport(backupPath);
            TempData["message"] = $"Список цен успешно сохранён в {backupName}";
            return View(nameof(Index));
        }

        #region Sections

        public IActionResult Sections() => View(Repository.GetMainSections().OrderBy(x => x.SequenceNumber));

        public IActionResult Section(int id) => View(Repository.GetSection(id));

        public IActionResult CreateSection(int parentId) => View("CreateOrEditSection",
            new Section() { ParentSection = Repository.Get<Section>(x => x.Id == parentId) });

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

        public IActionResult EditSection(int id) => View("CreateOrEditSection", Repository.Get<Section>(x => x.Id == id));

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
            View("CreateOrEditProduct", new Product(Repository.Get<Section>(x => x.Id == sectionId)));

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
            product.Section = Repository.Get<Section>(x => x.Id == sectionId);
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
            new ProductModel(Repository.Get<Section>(x => x.Id == sectionId), Repository.GetProduct(productId)));

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
            productModel.Section = Repository.Get<Section>(x => x.Id == sectionId);
            productModel.Product = Repository.Get<Product>(x => x.Id == productId);
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
        public async Task<IActionResult> UploadSectionPhoto(int id, IFormFile uploadedPhoto)
        {
            var section = Repository.GetSection(id);
            if (uploadedPhoto != null)
            {
                string sectionImageFullLink = section.GetImageFullLink();
                string path = HostingEnvironment.WebRootPath + sectionImageFullLink;
                TempData["message"] = $"Фото {sectionImageFullLink} загружено";
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadedPhoto.CopyToAsync(fileStream);
                }
            }

            return section.IsMainSection() ? RedirectToAction(nameof(Sections))
                : RedirectToAction(nameof(Section), new { id = section.ParentSection.Id });
        }

        [HttpPost]
        public async Task<IActionResult> UploadProductPhoto(int id, IFormFile uploadedPhoto)
        {
            var product = Repository.GetProduct(id);
            if (uploadedPhoto != null)
            {
                string productImageFullLink = product.GetImageFullLink();
                string path = HostingEnvironment.WebRootPath + productImageFullLink;
                TempData["message"] = $"Фото {productImageFullLink} загружено";
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadedPhoto.CopyToAsync(fileStream);
                }
            }

            return RedirectToAction(nameof(Section), new { id = product.Section.Id });
        }

        [HttpPost]
        public async Task<IActionResult> UploadProductModelPhoto(int id, IFormFile uploadedPhoto)
        {
            var productModel = Repository.GetProductModel(id);
            if (uploadedPhoto != null)
            {
                string productModelImageFullLink = productModel.GetImageFullLink();
                string path = HostingEnvironment.WebRootPath + productModelImageFullLink;
                TempData["message"] = $"Фото {productModelImageFullLink} загружено";
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadedPhoto.CopyToAsync(fileStream);
                }
            }

            return productModel.Section == null ? RedirectToAction(nameof(Product), new { id = productModel.Product.Id })
                : RedirectToAction(nameof(Section), new { id = productModel.Section.Id });
        }

        #endregion
    }
}
