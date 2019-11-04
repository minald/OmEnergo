using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OmEnergo.Infrastructure;
using OmEnergo.Infrastructure.Database;
using OmEnergo.Infrastructure.Excel;
using OmEnergo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Controllers
{
	[AdminAuthorization]
	public class AdminController : Controller
	{
		private Repository _Repository { get; set; }
		private FileManager _FileManager { get; set; }

		public AdminController(Repository repository, IHostingEnvironment hostingEnvironment)
		{
			_Repository = repository;
			_FileManager = new FileManager(repository, hostingEnvironment);
		}

		public override void OnActionExecuted(ActionExecutedContext context)
		{
			ViewData["Title"] = "Административная панель";
			base.OnActionExecuted(context);
		}

		#region Main page

		public IActionResult Index() => View();

		public IActionResult Configuration() => View(_Repository.GetAllConfigKeys());

		[HttpPost]
		public IActionResult SaveConfiguration(List<ConfigKey> configKeys)
		{
			_Repository.UpdateRange(configKeys);
			return RedirectToAction(nameof(Configuration));
		}

		public FileStreamResult CreateBackup([FromServices]ExcelWriter excelWriter)
		{
            MemoryStream excelFileStream = excelWriter.CreateExcelStream();
            string currentDatetime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string fullFileName = $"OmEnergoDB_{currentDatetime}.xlsx";
            return File(excelFileStream, ExcelWriter.XlsxMimeType, fullFileName);
        }

        [HttpPost]
        public IActionResult UploadExcelWithData([FromServices]ExcelDbUpdater excelDbUpdater, IFormFile uploadedFile)
        {
            try
            {
                if (uploadedFile == null)
                {
                    throw new Exception("Пожалуйста, выберите файл");
                }

                Stream excelFileStream = uploadedFile.OpenReadStream();
                excelDbUpdater.ReadExcelAndUpdateDb(excelFileStream);
                TempData["message"] = "Данные успешно обновлены";
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
		public IActionResult CreateThumbnails(int maxSize)
		{
			var commonObjects = new List<CommonObject>();
			commonObjects.AddRange(_Repository.GetAllSections() ?? new List<Section>());
			commonObjects.AddRange(_Repository.GetAllProducts() ?? new List<Product>());
			commonObjects.AddRange(_Repository.GetAllProductModels() ?? new List<ProductModel>());
			var imageThumbnailCreator = new ImageThumbnailCreator(maxSize);
			imageThumbnailCreator.Create(commonObjects);
			TempData["message"] = "Миниатюры изображений успешно созданы";
			return RedirectToAction(nameof(Index));
		}

		#endregion 

		#region Sections

		public IActionResult Sections() => View(_Repository.GetOrderedMainSections());

		public IActionResult Section(int id) => View(_Repository.GetSection(id));

		public IActionResult CreateSection(int parentId) => View("CreateOrEditSection",
			new Section() { ParentSection = _Repository.Get<Section>(x => x.Id == parentId) });

		[HttpPost]
		public IActionResult CreateSection(Section section, int? parentSectionId)
		{
			if (parentSectionId != null)
			{
				section.ParentSection = _Repository.GetSection(parentSectionId.Value);
				section.SequenceNumber = section.ParentSection.GetOrderedNestedObjects().Count() + 1;
			}
			else
			{
				section.SequenceNumber = _Repository.GetOrderedMainSections().Count() + 1;
			}

			section.SetEnglishNameIfEmpty();
			_Repository.Update(section);
			TempData["message"] = $"Секция {section.Name} создана";
			return section.IsMainSection() ? RedirectToAction(nameof(Sections))
				: RedirectToAction(nameof(Section), new { id = section.ParentSection.Id });
		}

		public IActionResult EditSection(int id) => View("CreateOrEditSection", _Repository.Get<Section>(x => x.Id == id));

		[HttpPost]
		public IActionResult EditSection(Section section, int? parentSectionId)
		{
			if (parentSectionId != null)
			{
				section.ParentSection = _Repository.GetSection(parentSectionId.Value);
			}

			section.SetEnglishNameIfEmpty();
			_Repository.UpdateSectionAndSynchronizeProperties(section);
			TempData["message"] = $"Секция {section.Name} изменена";
			return section.IsMainSection() ? RedirectToAction(nameof(Sections))
				: RedirectToAction(nameof(Section), new { id = section.ParentSection.Id });
		}

		[HttpPost]
		public IActionResult DeleteSection(int id)
		{
			var section = _Repository.GetSection(id);
			_Repository.Delete<Section>(id);
			TempData["message"] = $"Секция {section.Name} удалена";
			return Redirect(Request.Headers["Referer"].ToString());
		}

		#endregion

		#region Products

		public IActionResult Product(int id) => View(_Repository.GetProduct(id));

		public IActionResult CreateProduct(int sectionId) => 
			View("CreateOrEditProduct", new Product(_Repository.Get<Section>(x => x.Id == sectionId)));

		[HttpPost]
		public IActionResult CreateProduct(Product product, int? sectionId, params string[] values)
		{
			product.Section = _Repository.GetSection(sectionId.Value);
			product.SequenceNumber = product.Section.GetOrderedNestedObjects().Count() + 1;
			product.UpdatePropertyValues(values);
			product.SetEnglishNameIfEmpty();
			_Repository.Update(product);
			TempData["message"] = $"Продукт {product.Name} создан";
			return RedirectToAction(nameof(Section), new { id = product.Section.Id });
		}

		public IActionResult EditProduct(int id) => View("CreateOrEditProduct", _Repository.GetProduct(id));

		[HttpPost]
		public IActionResult EditProduct(Product product, int? sectionId, params string[] values)
		{
			product.Section = _Repository.Get<Section>(x => x.Id == sectionId);
			product.UpdatePropertyValues(values);
			product.SetEnglishNameIfEmpty();
			_Repository.Update(product);
			TempData["message"] = $"Продукт {product.Name} изменён";
			return RedirectToAction(nameof(Section), new { id = product.Section.Id });
		}

		[HttpPost]
		public IActionResult DeleteProduct(int id)
		{
			_Repository.Delete<Product>(id);
			TempData["message"] = $"Продукт удалён";
			return Redirect(Request.Headers["Referer"].ToString());
		}

		#endregion

		#region ProductModels

		public IActionResult CreateProductModel(int sectionId, int productId) => View("CreateOrEditProductModel",
			new ProductModel(_Repository.Get<Section>(x => x.Id == sectionId), _Repository.GetProduct(productId)));

		[HttpPost]
		public IActionResult CreateProductModel(ProductModel productModel, int? sectionId, int? productId, params string[] values)
		{
			productModel.Section = _Repository.GetSection(sectionId.GetValueOrDefault());
			productModel.Product = _Repository.GetProduct(productId.GetValueOrDefault());
			productModel.SequenceNumber =
				(sectionId == null ? productModel.Product.Models : productModel.Section.GetOrderedNestedObjects()).Count() + 1;
			productModel.UpdatePropertyValues(values);
			productModel.SetEnglishNameIfEmpty();
			_Repository.Update(productModel);
			TempData["message"] = $"Модель {productModel.Name} создана";
			return sectionId == null ? RedirectToAction(nameof(Product), new { id = productModel.Product.Id })
				: RedirectToAction(nameof(Section), new { id = productModel.Section.Id });
		}

		public IActionResult EditProductModel(int id) => 
			View("CreateOrEditProductModel", _Repository.GetProductModel(id));

		[HttpPost]
		public IActionResult EditProductModel(ProductModel productModel, int? sectionId, int? productId, params string[] values)
		{
			productModel.Section = _Repository.Get<Section>(x => x.Id == sectionId);
			productModel.Product = _Repository.Get<Product>(x => x.Id == productId);
			productModel.UpdatePropertyValues(values);
			productModel.SetEnglishNameIfEmpty();
			_Repository.Update(productModel);
			TempData["message"] = $"Модель {productModel.Name} изменена";
			return sectionId == null ? RedirectToAction(nameof(Product), new { id = productModel.Product.Id })
				: RedirectToAction(nameof(Section), new { id = productModel.Section.Id });
		}

		[HttpPost]
		public IActionResult DeleteProductModel(int id)
		{
			_Repository.Delete<ProductModel>(id);
			TempData["message"] = $"Модель удалена";
			return Redirect(Request.Headers["Referer"].ToString());
		}

		#endregion

		#region FileManager

		public IActionResult FileManager(string englishName)
		{
			var commonObject = _Repository.GetObjectByEnglishName(englishName);
			return View(commonObject);
		}

		[HttpPost]
		public async Task<IActionResult> UploadFileAsync(string englishName, IFormFile uploadedFile)
		{
			try
			{
				await _FileManager.UploadFileAsync(englishName, uploadedFile);
			}
			catch (Exception ex)
			{
				TempData["message"] = ex.Message;
			}

			return Redirect(Request.Headers["Referer"].ToString());
		}

		[HttpPost]
		public IActionResult DeleteFile(string path, string englishName)
		{
			_FileManager.DeleteFile(path, englishName);
			return Redirect(Request.Headers["Referer"].ToString());
		}

		[HttpPost]
		public IActionResult MakeImageMain(string path, string englishName)
		{
			_FileManager.MakeImageMain(path, englishName);
			return Redirect(Request.Headers["Referer"].ToString());
		}

        #endregion

        //Method is necessary for validation of create/edit actions
        public IActionResult IsNewEnglishName(string englishName, int id)
        {
            var obj = _Repository.GetObjectByEnglishName(englishName);
            bool isNew = obj == null ? true : obj?.Id == id;
            return Json(isNew);
        }
    }
}
