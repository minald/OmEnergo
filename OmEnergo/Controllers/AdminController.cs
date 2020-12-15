﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
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
	[Authorize]
	public class AdminController : Controller
	{
		private readonly SectionRepository sectionRepository;
		private readonly ProductRepository productRepository;
		private readonly ProductModelRepository productModelRepository;
		private readonly ConfigKeyRepository configKeyRepository;
		private readonly CompoundRepository compoundRepository;
		private readonly AdminFileManager adminFileManager;
		private readonly ILogger<AdminController> logger;
		private readonly IStringLocalizer localizer;

		public AdminController(SectionRepository sectionRepository, 
			ProductRepository productRepository, 
			ProductModelRepository productModelRepository, 
			ConfigKeyRepository configKeyRepository, 
			CompoundRepository compoundRepository,
			AdminFileManager adminFileManager, 
			ILogger<AdminController> logger, 
			IStringLocalizer localizer)
		{
			this.sectionRepository = sectionRepository;
			this.productRepository = productRepository;
			this.productModelRepository = productModelRepository;
			this.configKeyRepository = configKeyRepository;
			this.compoundRepository = compoundRepository;
			this.adminFileManager = adminFileManager;
			this.logger = logger;
			this.localizer = localizer;
		}

		public override void OnActionExecuted(ActionExecutedContext context)
		{
			ViewData["Title"] = localizer["AdministrativePanel"];
			base.OnActionExecuted(context);
		}

		#region Main page

		public IActionResult Index() => View();

		public IActionResult Configuration() => View(configKeyRepository.GetAll<ConfigKey>());

		[HttpPost]
		public IActionResult SaveConfiguration(List<ConfigKey> configKeys)
		{
			configKeyRepository.UpdateRange(configKeys);
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
					throw new Exception(localizer["PleaseSelectAFile"]);
				}

				using var excelFileStream = uploadedFile.OpenReadStream();
				excelDbUpdater.ReadExcelAndUpdateDb(excelFileStream);
				TempData["message"] = localizer["DataWasSuccessfullyUpdated"].Value;
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
			commonObjects.AddRange(sectionRepository.GetAll<Section>() ?? new List<Section>());
			commonObjects.AddRange(productRepository.GetAll<Product>() ?? new List<Product>());
			commonObjects.AddRange(productModelRepository.GetAll<ProductModel>() ?? new List<ProductModel>());
			var imageThumbnailCreator = new ImageThumbnailCreator(maxSize);
			imageThumbnailCreator.Create(commonObjects);
			TempData["message"] = localizer["ImageThumbnailsWereSuccessfullyCreated"].Value;
			return RedirectToAction(nameof(Index));
		}

		#endregion 

		#region Sections

		public IActionResult Sections() => View(sectionRepository.GetOrderedMainSections());

		public IActionResult Section(int id) => View(sectionRepository.GetById<Section>(id));

		public IActionResult CreateSection(int parentId) => View("CreateOrEditSection",
			new Section() { ParentSection = sectionRepository.Get<Section>(x => x.Id == parentId) });

		[HttpPost]
		public IActionResult CreateSection(Section section, int? parentSectionId)
		{
			if (parentSectionId != null)
			{
				section.ParentSection = sectionRepository.GetById<Section>(parentSectionId.Value);
				section.SequenceNumber = section.ParentSection.GetOrderedNestedObjects().Count() + 1;
			}
			else
			{
				section.SequenceNumber = sectionRepository.GetOrderedMainSections().Count() + 1;
			}

			section.SetEnglishNameIfEmpty();
			sectionRepository.Update(section);
			TempData["message"] = $"{localizer["Section"]} {section.Name} {localizer["WasDeleted_f"]}";
			return section.IsMainSection() ? RedirectToAction(nameof(Sections))
				: RedirectToAction(nameof(Section), new { id = section.ParentSection.Id });
		}

		public IActionResult EditSection(int id) => View("CreateOrEditSection", sectionRepository.Get<Section>(x => x.Id == id));

		[HttpPost]
		public IActionResult EditSection(Section section, int? parentSectionId)
		{
			if (parentSectionId != null)
			{
				section.ParentSection = sectionRepository.GetById<Section>(parentSectionId.Value);
			}

			section.SetEnglishNameIfEmpty();
			compoundRepository.UpdateSectionAndSynchronizeProperties(section);
			TempData["message"] = $"{localizer["Section"]} {section.Name} {localizer["WasChanged_f"]}";
			return section.IsMainSection() ? RedirectToAction(nameof(Sections))
				: RedirectToAction(nameof(Section), new { id = section.ParentSection.Id });
		}

		[HttpPost]
		public IActionResult DeleteSection(int id)
		{
			var section = sectionRepository.GetById<Section>(id);
			sectionRepository.Delete<Section>(id);
			TempData["message"] = $"{localizer["Section"]} {section.Name} {localizer["WasDeleted_f"]}";
			return Redirect(Request.Headers["Referer"].ToString());
		}

		#endregion

		#region Products

		public IActionResult Product(int id) => View(productRepository.GetById<Product>(id));

		public IActionResult CreateProduct(int sectionId) => 
			View("CreateOrEditProduct", new Product(sectionRepository.Get<Section>(x => x.Id == sectionId)));

		[HttpPost]
		public IActionResult CreateProduct(Product product, int? sectionId, params string[] values)
		{
			product.Section = sectionRepository.GetById<Section>(sectionId.Value);
			product.SequenceNumber = product.Section.GetOrderedNestedObjects().Count() + 1;
			product.UpdatePropertyValues(values);
			product.SetEnglishNameIfEmpty();
			productRepository.Update(product);
			TempData["message"] = $"{localizer["Product"]} {product.Name} {localizer["WasDeleted_m"]}";
			return RedirectToAction(nameof(Section), new { id = product.Section.Id });
		}

		public IActionResult EditProduct(int id) => View("CreateOrEditProduct", productRepository.GetById<Product>(id));

		[HttpPost]
		public IActionResult EditProduct(Product product, int? sectionId, params string[] values)
		{
			product.Section = sectionRepository.Get<Section>(x => x.Id == sectionId);
			product.UpdatePropertyValues(values);
			product.SetEnglishNameIfEmpty();
			productRepository.Update(product);
			TempData["message"] = $"{localizer["Product"]} {product.Name} {localizer["WasChanged_m"]}";
			return RedirectToAction(nameof(Section), new { id = product.Section.Id });
		}

		[HttpPost]
		public IActionResult DeleteProduct(int id)
		{
			productRepository.Delete<Product>(id);
			TempData["message"] = $"{localizer["Product"]} {localizer["WasDeleted_m"]}";
			return Redirect(Request.Headers["Referer"].ToString());
		}

		#endregion

		#region ProductModels

		public IActionResult CreateProductModel(int sectionId, int productId) => View("CreateOrEditProductModel",
			new ProductModel(sectionRepository.Get<Section>(x => x.Id == sectionId), productRepository.GetById<Product>(productId)));

		[HttpPost]
		public IActionResult CreateProductModel(ProductModel productModel, int? sectionId, int? productId, params string[] values)
		{
			productModel.Section = sectionRepository.GetById<Section>(sectionId.GetValueOrDefault());
			productModel.Product = productRepository.GetById<Product>(productId.GetValueOrDefault());
			productModel.SequenceNumber =
				(sectionId == null ? productModel.Product.Models : productModel.Section.GetOrderedNestedObjects()).Count() + 1;
			productModel.UpdatePropertyValues(values);
			productModel.SetEnglishNameIfEmpty();
			productModelRepository.Update(productModel);
			TempData["message"] = $"{localizer["Model"]} {productModel.Name} {localizer["WasDeleted_f"]}";
			return sectionId == null ? RedirectToAction(nameof(Product), new { id = productModel.Product.Id })
				: RedirectToAction(nameof(Section), new { id = productModel.Section.Id });
		}

		public IActionResult EditProductModel(int id) => 
			View("CreateOrEditProductModel", productModelRepository.GetById<ProductModel>(id));

		[HttpPost]
		public IActionResult EditProductModel(ProductModel productModel, int? sectionId, int? productId, params string[] values)
		{
			productModel.Section = sectionRepository.Get<Section>(x => x.Id == sectionId);
			productModel.Product = productRepository.Get<Product>(x => x.Id == productId);
			productModel.UpdatePropertyValues(values);
			productModel.SetEnglishNameIfEmpty();
			productModelRepository.Update(productModel);
			TempData["message"] = $"{localizer["Model"]} {productModel.Name} {localizer["WasChanged_f"]}";
			return sectionId == null ? RedirectToAction(nameof(Product), new { id = productModel.Product.Id })
				: RedirectToAction(nameof(Section), new { id = productModel.Section.Id });
		}

		[HttpPost]
		public IActionResult DeleteProductModel(int id)
		{
			productModelRepository.Delete<ProductModel>(id);
			TempData["message"] = $"{localizer["Model"]} {localizer["WasDeleted_f"]}";
			return Redirect(Request.Headers["Referer"].ToString());
		}

		#endregion


		#region FileManager

		public IActionResult FileManager(string englishName)
		{
			var commonObject = compoundRepository.GetObjectByEnglishName(englishName);
			return View(commonObject);
		}

		[HttpPost]
		public async Task<IActionResult> UploadFileAsync(string englishName, IFormFile uploadedFile)
		{
			try
			{
				await adminFileManager.UploadFileAsync(englishName, uploadedFile);
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
			adminFileManager.DeleteFile(path, englishName);
			return Redirect(Request.Headers["Referer"].ToString());
		}

		[HttpPost]
		public IActionResult MakeImageMain(string path, string englishName)
		{
			adminFileManager.MakeImageMain(path, englishName);
			return Redirect(Request.Headers["Referer"].ToString());
		}

		#endregion

		//Method is necessary for validation of create/edit actions
		public IActionResult IsNewEnglishName(string englishName, int id)
		{
			var obj = compoundRepository.GetObjectByEnglishName(englishName);
			var isNew = obj == null || obj?.Id == id;
			return Json(isNew);
		}
	}
}
