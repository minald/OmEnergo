using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OmEnergo.Infrastructure;
using OmEnergo.Infrastructure.Database;
using OmEnergo.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Controllers
{
	[AdminAuthorization]
	public class AdminController : Controller
	{
		private Repository Repository { get; set; }
		private IHostingEnvironment HostingEnvironment { get; set; }
		private FileManager _FileManager { get; set; }

		public AdminController(Repository repository, IHostingEnvironment hostingEnvironment)
		{
			Repository = repository;
			HostingEnvironment = hostingEnvironment;
			_FileManager = new FileManager(repository, hostingEnvironment);
		}

		public override void OnActionExecuted(ActionExecutedContext context)
		{
			ViewData["Title"] = "Административная панель";
			base.OnActionExecuted(context);
		}

		#region Main page

		public IActionResult Index() => View();

		public IActionResult Configuration() => View(Repository.GetAllConfigKeys());

		[HttpPost]
		public IActionResult SaveConfiguration(List<ConfigKey> configKeys)
		{
			Repository.UpdateRange(configKeys);
			return RedirectToAction(nameof(Configuration));
		}

		public IActionResult CreateBackup([FromServices]ExcelReportBuilder excelReportBuilder)
		{
			var fileStream = excelReportBuilder.CreateDatabaseBackup();
			return GetExcelFile(fileStream, "OmEnergoDB");
		}

		public IActionResult GetPricesReport([FromServices]ExcelReportBuilder excelReportBuilder)
		{
			var fileStream = excelReportBuilder.CreatePricesReport();
			return GetExcelFile(fileStream, "OmEnergoPrices");
		}

		private FileStreamResult GetExcelFile(Stream stream, string mainFileNamePart)
		{
			string currentDatetime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
			string fullFileName = $"{mainFileNamePart}_{currentDatetime}.xlsx";
			return File(stream, ExcelReportBuilder.XlsxMimeType, fullFileName);
		}

		[HttpPost]
		public IActionResult CreateThumbnails(int maxSize)
		{
			var commonObjects = new List<CommonObject>();
			commonObjects.AddRange(Repository.GetAllSections() ?? new List<Section>());
			commonObjects.AddRange(Repository.GetAllProducts() ?? new List<Product>());
			commonObjects.AddRange(Repository.GetAllProductModels() ?? new List<ProductModel>());
			var imageThumbnailCreator = new ImageThumbnailCreator(maxSize);
			imageThumbnailCreator.Create(commonObjects);
			TempData["message"] = "Миниатюры изображений успешно созданы";
			return RedirectToAction(nameof(Index));
		}

		#endregion 

		#region Sections

		public IActionResult Sections() => View(Repository.GetOrderedMainSections());

		public IActionResult Section(int id) => View(Repository.GetSection(id));

		public IActionResult CreateSection(int parentId) => View("CreateOrEditSection",
			new Section() { ParentSection = Repository.Get<Section>(x => x.Id == parentId) });

		[HttpPost]
		public IActionResult CreateSection(Section section, int? parentSectionId)
		{
			if (parentSectionId != null)
			{
				section.ParentSection = Repository.GetSection(parentSectionId.Value);
				section.SequenceNumber = section.ParentSection.GetOrderedNestedObjects().Count() + 1;
			}
			else
			{
				section.SequenceNumber = Repository.GetOrderedMainSections().Count() + 1;
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
			product.SequenceNumber = product.Section.GetOrderedNestedObjects().Count() + 1;
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
				(sectionId == null ? productModel.Product.Models : productModel.Section.GetOrderedNestedObjects()).Count() + 1;
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

		#endregion

		#region FileManager

		public IActionResult FileManager(string englishName)
		{
			var commonObject = Repository.GetObjectByEnglishName(englishName);
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

		//Method is necessary for validation of create/edit 
		public IActionResult IsNewEnglishName(string englishName) 
			=> Json(Repository.GetObjectByEnglishName(englishName) == null);
	}
}
