using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using OmEnergo.Infrastructure;
using OmEnergo.Infrastructure.Database;
using OmEnergo.Infrastructure.Excel;
using OmEnergo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Controllers
{
	[AdminAuthorization]
	public class AdminController : Controller
	{
		private readonly Repository repository;
		private readonly FileManager fileManager;
		private readonly ILogger<AdminController> logger;

		public AdminController(Repository repository, IHostingEnvironment hostingEnvironment, ILogger<AdminController> logger)
		{
			this.repository = repository;
			fileManager = new FileManager(repository, hostingEnvironment);
			this.logger = logger;
		}

		public override void OnActionExecuted(ActionExecutedContext context)
		{
			ViewData["Title"] = "Административная панель";
			base.OnActionExecuted(context);
		}

		#region Main page

		public IActionResult Index() => View();

		public IActionResult Configuration() => View(repository.GetAllConfigKeys());

		[HttpPost]
		public IActionResult SaveConfiguration(List<ConfigKey> configKeys)
		{
			repository.UpdateRange(configKeys);
			return RedirectToAction(nameof(Configuration));
		}

		public FileStreamResult CreateBackup([FromServices]ExcelWriter excelWriter)
		{
			using var excelFileStream = excelWriter.CreateExcelStream();
			var currentDatetime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
			var fullFileName = $"OmEnergoDB_{currentDatetime}.xlsx";
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

				using var excelFileStream = uploadedFile.OpenReadStream();
				excelDbUpdater.ReadExcelAndUpdateDb(excelFileStream);
				TempData["message"] = "Данные успешно обновлены";
			}
			catch (Exception ex)
			{
				TempData["message"] = ex.Message;
				logger.LogError($"AdminController: {ex.Message}");
			}

			return Redirect(Request.Headers["Referer"].ToString());
		}

		[HttpPost]
		public IActionResult CreateThumbnails(int maxSize)
		{
			var commonObjects = new List<CommonObject>();
			commonObjects.AddRange(repository.GetAllSections() ?? new List<Section>());
			commonObjects.AddRange(repository.GetAllProducts() ?? new List<Product>());
			commonObjects.AddRange(repository.GetAllProductModels() ?? new List<ProductModel>());
			var imageThumbnailCreator = new ImageThumbnailCreator(maxSize);
			imageThumbnailCreator.Create(commonObjects);
			TempData["message"] = "Миниатюры изображений успешно созданы";
			return RedirectToAction(nameof(Index));
		}

		#endregion 

		#region Sections

		public IActionResult Sections() => View(repository.GetOrderedMainSections());

		public IActionResult Section(int id) => View(repository.GetSection(id));

		public IActionResult CreateSection(int parentId) => View("CreateOrEditSection",
			new Section() { ParentSection = repository.Get<Section>(x => x.Id == parentId) });

		[HttpPost]
		public IActionResult CreateSection(Section section, int? parentSectionId)
		{
			if (parentSectionId != null)
			{
				section.ParentSection = repository.GetSection(parentSectionId.Value);
				section.SequenceNumber = section.ParentSection.GetOrderedNestedObjects().Count() + 1;
			}
			else
			{
				section.SequenceNumber = repository.GetOrderedMainSections().Count() + 1;
			}

			section.SetEnglishNameIfEmpty();
			repository.Update(section);
			TempData["message"] = $"Секция {section.Name} создана";
			return section.IsMainSection() ? RedirectToAction(nameof(Sections))
				: RedirectToAction(nameof(Section), new { id = section.ParentSection.Id });
		}

		public IActionResult EditSection(int id) => View("CreateOrEditSection", repository.Get<Section>(x => x.Id == id));

		[HttpPost]
		public IActionResult EditSection(Section section, int? parentSectionId)
		{
			if (parentSectionId != null)
			{
				section.ParentSection = repository.GetSection(parentSectionId.Value);
			}

			section.SetEnglishNameIfEmpty();
			repository.UpdateSectionAndSynchronizeProperties(section);
			TempData["message"] = $"Секция {section.Name} изменена";
			return section.IsMainSection() ? RedirectToAction(nameof(Sections))
				: RedirectToAction(nameof(Section), new { id = section.ParentSection.Id });
		}

		[HttpPost]
		public IActionResult DeleteSection(int id)
		{
			var section = repository.GetSection(id);
			repository.Delete<Section>(id);
			TempData["message"] = $"Секция {section.Name} удалена";
			return Redirect(Request.Headers["Referer"].ToString());
		}

		#endregion

		#region Products

		public IActionResult Product(int id) => View(repository.GetProduct(id));

		public IActionResult CreateProduct(int sectionId) => 
			View("CreateOrEditProduct", new Product(repository.Get<Section>(x => x.Id == sectionId)));

		[HttpPost]
		public IActionResult CreateProduct(Product product, int? sectionId, params string[] values)
		{
			product.Section = repository.GetSection(sectionId.Value);
			product.SequenceNumber = product.Section.GetOrderedNestedObjects().Count() + 1;
			product.UpdatePropertyValues(values);
			product.SetEnglishNameIfEmpty();
			repository.Update(product);
			TempData["message"] = $"Продукт {product.Name} создан";
			return RedirectToAction(nameof(Section), new { id = product.Section.Id });
		}

		public IActionResult EditProduct(int id) => View("CreateOrEditProduct", repository.GetProduct(id));

		[HttpPost]
		public IActionResult EditProduct(Product product, int? sectionId, params string[] values)
		{
			product.Section = repository.Get<Section>(x => x.Id == sectionId);
			product.UpdatePropertyValues(values);
			product.SetEnglishNameIfEmpty();
			repository.Update(product);
			TempData["message"] = $"Продукт {product.Name} изменён";
			return RedirectToAction(nameof(Section), new { id = product.Section.Id });
		}

		[HttpPost]
		public IActionResult DeleteProduct(int id)
		{
			repository.Delete<Product>(id);
			TempData["message"] = $"Продукт удалён";
			return Redirect(Request.Headers["Referer"].ToString());
		}

		#endregion

		#region ProductModels

		public IActionResult CreateProductModel(int sectionId, int productId) => View("CreateOrEditProductModel",
			new ProductModel(repository.Get<Section>(x => x.Id == sectionId), repository.GetProduct(productId)));

		[HttpPost]
		public IActionResult CreateProductModel(ProductModel productModel, int? sectionId, int? productId, params string[] values)
		{
			productModel.Section = repository.GetSection(sectionId.GetValueOrDefault());
			productModel.Product = repository.GetProduct(productId.GetValueOrDefault());
			productModel.SequenceNumber =
				(sectionId == null ? productModel.Product.Models : productModel.Section.GetOrderedNestedObjects()).Count() + 1;
			productModel.UpdatePropertyValues(values);
			productModel.SetEnglishNameIfEmpty();
			repository.Update(productModel);
			TempData["message"] = $"Модель {productModel.Name} создана";
			return sectionId == null ? RedirectToAction(nameof(Product), new { id = productModel.Product.Id })
				: RedirectToAction(nameof(Section), new { id = productModel.Section.Id });
		}

		public IActionResult EditProductModel(int id) => 
			View("CreateOrEditProductModel", repository.GetProductModel(id));

		[HttpPost]
		public IActionResult EditProductModel(ProductModel productModel, int? sectionId, int? productId, params string[] values)
		{
			productModel.Section = repository.Get<Section>(x => x.Id == sectionId);
			productModel.Product = repository.Get<Product>(x => x.Id == productId);
			productModel.UpdatePropertyValues(values);
			productModel.SetEnglishNameIfEmpty();
			repository.Update(productModel);
			TempData["message"] = $"Модель {productModel.Name} изменена";
			return sectionId == null ? RedirectToAction(nameof(Product), new { id = productModel.Product.Id })
				: RedirectToAction(nameof(Section), new { id = productModel.Section.Id });
		}

		[HttpPost]
		public IActionResult DeleteProductModel(int id)
		{
			repository.Delete<ProductModel>(id);
			TempData["message"] = $"Модель удалена";
			return Redirect(Request.Headers["Referer"].ToString());
		}

		#endregion


		#region FileManager

		public IActionResult FileManager(string englishName)
		{
			var commonObject = repository.GetObjectByEnglishName(englishName);
			return View(commonObject);
		}

		[HttpPost]
		public async Task<IActionResult> UploadFileAsync(string englishName, IFormFile uploadedFile)
		{
			try
			{
				await fileManager.UploadFileAsync(englishName, uploadedFile);
			}
			catch (Exception ex)
			{
				TempData["message"] = ex.Message;
				logger.LogError($"AdminController: {ex.Message}");
			}

			return Redirect(Request.Headers["Referer"].ToString());
		}

		[HttpPost]
		public IActionResult DeleteFile(string path, string englishName)
		{
			fileManager.DeleteFile(path, englishName);
			return Redirect(Request.Headers["Referer"].ToString());
		}

		[HttpPost]
		public IActionResult MakeImageMain(string path, string englishName)
		{
			fileManager.MakeImageMain(path, englishName);
			return Redirect(Request.Headers["Referer"].ToString());
		}

		#endregion

		//Method is necessary for validation of create/edit actions
		public IActionResult IsNewEnglishName(string englishName, int id)
		{
			var obj = repository.GetObjectByEnglishName(englishName);
			var isNew = obj == null || obj?.Id == id;
			return Json(isNew);
		}
	}
}
