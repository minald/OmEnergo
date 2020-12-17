using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OmEnergo.Infrastructure;
using OmEnergo.Infrastructure.Database;
using OmEnergo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Services
{
	public class AdminFileManagerService
	{
		private readonly FileManager fileManager;

		private readonly CompoundRepository compoundRepository;

		private readonly IStringLocalizer localizer;

		private readonly static List<string> supportedForPreviewDocumentExtensions = new List<string>() { "pdf", "txt" };
		private readonly static List<string> supportedImageExtensions = new List<string>() { "jpg", "jpeg", "png" };
		private readonly static List<string> supportedDocumentExtensions = FileManager.SupportedDocumentExtensionsAndMimeTypes.Keys.ToList();

		public AdminFileManagerService(FileManager fileManager, CompoundRepository compoundRepository, IStringLocalizer localizer)
		{
			this.fileManager = fileManager;
			this.compoundRepository = compoundRepository;
			this.localizer = localizer;
		}

		public static bool IsTheMainImage(string imagePath) => !imagePath.Contains("_");

		public static bool IsDocumentCanBePreviewed(string path)
		{
			var fileExtension = FileManager.GetExtensionWithoutDot(path);
			return supportedForPreviewDocumentExtensions.Contains(fileExtension);
		}

		public static List<string> GetFullPathsOfObjectImages(CommonObject commonObject) => GetFullPathsOfObjectFiles(commonObject)
			.Where(x => supportedImageExtensions.Contains(FileManager.GetExtensionWithoutDot(x))).ToList();

		public static List<string> GetFullPathsOfObjectDocuments(CommonObject commonObject) => GetFullPathsOfObjectFiles(commonObject)
			.Where(x => supportedDocumentExtensions.Contains(FileManager.GetExtensionWithoutDot(x))).ToList();

		public static List<string> GetFullPathsOfObjectFiles(CommonObject commonObject)
		{
			var directoryPath = FileManager.MakeFullPathFromRelative(commonObject.GetDirectoryPath());
			var namePatternOfMainImage = commonObject.GetNamePatternOfMainImage();
			var namePatternOfOtherFiles = commonObject.GetNamePatternOfAllFilesExceptMainImage();
			var namePatterns = new List<string> { namePatternOfMainImage, namePatternOfOtherFiles };
			return FileManager.GetFullFilesPathsByNamePatterns(directoryPath, namePatterns);
		}

		public async Task UploadFileAsync(string objectEnglishName, IFormFile uploadedFile)
		{
			var obj = await compoundRepository.GetObjectByEnglishNameAsync(objectEnglishName);
			var path = GetTargetPath(obj, uploadedFile);
			await fileManager.UploadFileAsync(path, uploadedFile);
		}

		public async Task DeleteFileAsync(string deletedFileFullPath, string objectEnglishName)
		{
			var obj = await compoundRepository.GetObjectByEnglishNameAsync(objectEnglishName);
			fileManager.DeleteFile(deletedFileFullPath);
			var mainImageFullPath = FileManager.MakeFullPathFromRelative(obj.GetMainImagePath());
			if (deletedFileFullPath == mainImageFullPath)
			{
				MakeSecondaryImageMain(obj, mainImageFullPath);
			}
		}

		public async Task MakeImageMainAsync(string newMainImageFullPath, string objectEnglishName)
		{
			var obj = await compoundRepository.GetObjectByEnglishNameAsync(objectEnglishName);
			var oldMainImageFullPath = FileManager.MakeFullPathFromRelative(obj.GetMainImagePath());
			if (newMainImageFullPath != oldMainImageFullPath)
			{
				string newSecondaryImageFullPath = GetNewSecondaryImagePath(oldMainImageFullPath, obj.Id);
				fileManager.RenameFile(oldMainImageFullPath, newSecondaryImageFullPath);
				fileManager.RenameFile(newMainImageFullPath, oldMainImageFullPath);
			}
		}

		private string GetTargetPath(CommonObject obj, IFormFile uploadedFile)
		{
			var uploadedFileExtension = FileManager.GetExtensionWithoutDot(uploadedFile.FileName);
			if (supportedDocumentExtensions.Contains(uploadedFileExtension))
			{
				return FileManager.MakeFullPathFromRelative(obj.GetDocumentPath(uploadedFile.FileName));
			}
			else if (supportedImageExtensions.Contains(uploadedFileExtension))
			{
				var mainImageFullPath = FileManager.MakeFullPathFromRelative(obj.GetMainImagePath());
				return File.Exists(mainImageFullPath) ? GetNewSecondaryImagePath(mainImageFullPath, obj.Id) : mainImageFullPath;
			}
			else
			{
				throw new FormatException(localizer["UploadFileFormatIsNotSupported"].Value
					.Replace("{{supportedDocumentExtensions}}", String.Join(", ", supportedDocumentExtensions))
					.Replace("{{supportedImageExtensions}}", String.Join(", ", supportedImageExtensions)));
			}
		}

		private void MakeSecondaryImageMain(CommonObject obj, string mainImagePath)
		{
			var secondaryImagePath = GetFullPathsOfObjectImages(obj).FirstOrDefault(p => p != mainImagePath);
			fileManager.RenameFile(secondaryImagePath, mainImagePath);
		}

		private string GetNewSecondaryImagePath(string path, int id) => path.Replace($"{id}.", $"{id}_{Guid.NewGuid()}.");
	}
}
