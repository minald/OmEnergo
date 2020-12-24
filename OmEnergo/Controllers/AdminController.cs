using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OmEnergo.Infrastructure.Excel;
using OmEnergo.Models;
using OmEnergo.Resources;
using OmEnergo.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OmEnergo.Controllers
{
	[Authorize]
	public class AdminController : Controller
	{
		private readonly AdminService adminService;
		private readonly AdminFileManagerService adminFileManagerService;
		private readonly ExcelService excelService;
		private readonly RepositoryService repositoryService;
		private readonly ILogger<AdminController> logger;
		private readonly IStringLocalizer localizer;

		public AdminController(AdminService adminService,
			AdminFileManagerService adminFileManagerService,
			ExcelService excelService,
			RepositoryService repositoryService,
			ILogger<AdminController> logger, 
			IStringLocalizer localizer)
		{
			
			this.adminService = adminService;
			this.adminFileManagerService = adminFileManagerService;
			this.excelService = excelService;
			this.repositoryService = repositoryService;
			this.logger = logger;
			this.localizer = localizer;
		}

		public override void OnActionExecuted(ActionExecutedContext context)
		{
			ViewData["Title"] = localizer[nameof(SharedResource.AdministrativePanel)];
			base.OnActionExecuted(context);
		}

		#region Main page

		public IActionResult Index() => View();

		public IActionResult Configuration() => View(repositoryService.GetAllConfigKeys());

		[HttpPost]
		public async Task<IActionResult> SaveConfiguration(List<ConfigKey> configKeys)
		{
			await repositoryService.UpdateConfigKeysAsync(configKeys);
			return RedirectToAction(nameof(Configuration));
		}

		public FileStreamResult CreateBackup()
		{
			var excelFileStream = excelService.GetBackupFileStream();
			var fullFileName = excelService.GetBackupFileName();
			return File(excelFileStream, ExcelWriter.XlsxMimeType, fullFileName);
		}

		[HttpPost]
		public async Task<IActionResult> UploadExcelWithData(IFormFile uploadedFile)
		{
			try
			{
				if (uploadedFile == null)
				{
					throw new ArgumentNullException(localizer[nameof(SharedResource.PleaseSelectAFile)]);
				}

				await excelService.UploadExcelAsync(uploadedFile);
				TempData["message"] = localizer[nameof(SharedResource.DataWasSuccessfullyUpdated)].Value;
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
			adminService.CreateThumbnails(maxSize);
			TempData["message"] = localizer[nameof(SharedResource.ImageThumbnailsWereSuccessfullyCreated)].Value;
			return RedirectToAction(nameof(Index));
		}

		#endregion 

		#region Sections

		public async Task<IActionResult> Sections() => View(await repositoryService.GetOrderedMainSectionsAsync());

		public IActionResult Section(int id) => View(repositoryService.GetFullSectionById(id));

		public IActionResult CreateSection(int parentId) => View("CreateOrEditSection",
			new Section() { ParentSection = repositoryService.GetSectionById(parentId) });

		[HttpPost]
		public async Task<IActionResult> CreateSection(Section section, int? parentSectionId)
		{
			await adminService.CreateSectionAsync(section, parentSectionId);
			TempData["message"] = $"{localizer[nameof(SharedResource.Section)]} {section.Name} {localizer[nameof(SharedResource.WasDeleted_f)]}";
			return section.IsMainSection() ? RedirectToAction(nameof(Sections))
				: RedirectToAction(nameof(Section), new { id = section.ParentSection.Id });
		}

		public IActionResult EditSection(int id) => View("CreateOrEditSection", repositoryService.GetSectionById(id));

		[HttpPost]
		public async Task<IActionResult> EditSection(Section section, int? parentSectionId)
		{
			await adminService.EditSectionAsync(section, parentSectionId);
			TempData["message"] = $"{localizer[nameof(SharedResource.Section)]} {section.Name} {localizer[nameof(SharedResource.WasChanged_f)]}";
			return section.IsMainSection() ? RedirectToAction(nameof(Sections))
				: RedirectToAction(nameof(Section), new { id = section.ParentSection.Id });
		}

		[HttpPost]
		public async Task<IActionResult> DeleteSection(int id)
		{
			string sectionName = await adminService.DeleteSectionAsync(id);
			TempData["message"] = $"{localizer[nameof(SharedResource.Section)]} {sectionName} {localizer[nameof(SharedResource.WasDeleted_f)]}";
			return Redirect(Request.Headers["Referer"].ToString());
		}

		#endregion

		#region Products

		public IActionResult Product(int id) => View(repositoryService.GetProductById(id));

		public IActionResult CreateProduct(int sectionId) => 
			View("CreateOrEditProduct", new Product(repositoryService.GetSectionById(sectionId)));

		[HttpPost]
		public async Task<IActionResult> CreateProduct(Product product, int? sectionId, params string[] propertyValues)
		{
			await adminService.CreateProductAsync(product, sectionId, propertyValues);
			TempData["message"] = $"{localizer[nameof(SharedResource.Product)]} {product.Name} {localizer[nameof(SharedResource.WasDeleted_m)]}";
			return RedirectToAction(nameof(Section), new { id = product.Section.Id });
		}

		public IActionResult EditProduct(int id) => View("CreateOrEditProduct", repositoryService.GetProductById(id));

		[HttpPost]
		public async Task<IActionResult> EditProduct(Product product, int? sectionId, params string[] propertyValues)
		{
			await adminService.EditProductAsync(product, sectionId, propertyValues);
			TempData["message"] = $"{localizer[nameof(SharedResource.Product)]} {product.Name} {localizer[nameof(SharedResource.WasChanged_m)]}";
			return RedirectToAction(nameof(Section), new { id = product.Section.Id });
		}

		[HttpPost]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			await adminService.DeleteProductAsync(id);
			TempData["message"] = $"{localizer[nameof(SharedResource.Product)]} {localizer[nameof(SharedResource.WasDeleted_m)]}";
			return Redirect(Request.Headers["Referer"].ToString());
		}

		#endregion

		#region ProductModels

		public IActionResult CreateProductModel(int sectionId, int productId) => View("CreateOrEditProductModel",
			new ProductModel(repositoryService.GetSectionById(sectionId), repositoryService.GetProductById(productId)));

		[HttpPost]
		public async Task<IActionResult> CreateProductModel(ProductModel productModel, int? sectionId, int? productId, params string[] propertyValues)
		{
			await adminService.CreateProductModelAsync(productModel, sectionId, productId, propertyValues);
			TempData["message"] = $"{localizer[nameof(SharedResource.Model)]} {productModel.Name} {localizer[nameof(SharedResource.WasDeleted_f)]}";
			return sectionId == null ? RedirectToAction(nameof(Product), new { id = productModel.Product.Id })
				: RedirectToAction(nameof(Section), new { id = productModel.Section.Id });
		}

		public IActionResult EditProductModel(int id) => 
			View("CreateOrEditProductModel", repositoryService.GetProductModelById(id));

		[HttpPost]
		public async Task<IActionResult> EditProductModel(ProductModel productModel, int? sectionId, int? productId, params string[] propertyValues)
		{
			await adminService.EditProductModelAsync(productModel, sectionId, productId, propertyValues);
			TempData["message"] = $"{localizer[nameof(SharedResource.Model)]} {productModel.Name} {localizer[nameof(SharedResource.WasChanged_f)]}";
			return sectionId == null ? RedirectToAction(nameof(Product), new { id = productModel.Product.Id })
				: RedirectToAction(nameof(Section), new { id = productModel.Section.Id });
		}

		[HttpPost]
		public async Task<IActionResult> DeleteProductModel(int id)
		{
			await adminService.DeleteProductModelAsync(id);
			TempData["message"] = $"{localizer[nameof(SharedResource.Model)]} {localizer[nameof(SharedResource.WasDeleted_f)]}";
			return Redirect(Request.Headers["Referer"].ToString());
		}

		#endregion


		#region FileManager

		public async Task<IActionResult> FileManager(string englishName)
		{
			var commonObject = await repositoryService.GetObjectByEnglishNameAsync(englishName);
			return View(commonObject);
		}

		[HttpPost]
		public async Task<IActionResult> UploadFile(string englishName, IFormFile uploadedFile)
		{
			try
			{
				await adminFileManagerService.UploadFileAsync(englishName, uploadedFile);
			}
			catch (Exception ex)
			{
				TempData["message"] = ex.Message;
				logger.LogError($"AdminController: {ex.Message}");
			}

			return Redirect(Request.Headers["Referer"].ToString());
		}

		[HttpPost]
		public async Task<IActionResult> DeleteFile(string path, string englishName)
		{
			await adminFileManagerService.DeleteFileAsync(path, englishName);
			return Redirect(Request.Headers["Referer"].ToString());
		}

		[HttpPost]
		public async Task<IActionResult> MakeImageMain(string path, string englishName)
		{
			await adminFileManagerService.MakeImageMainAsync(path, englishName);
			return Redirect(Request.Headers["Referer"].ToString());
		}

		#endregion

		//Method is necessary for validation of create/edit actions
		public async Task<IActionResult> IsNewEnglishName(string englishName, int id)
		{
			var obj = await repositoryService.GetObjectByEnglishNameAsync(englishName);
			var isNew = obj == null || obj?.Id == id;
			return Json(isNew);
		}
	}
}
