using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OmEnergo.Infrastructure.Database;
using OmEnergo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Infrastructure
{
	public class FileManager
	{
		#region Properties

		private readonly CompoundRepository compoundRepository;
		private readonly IStringLocalizer localizer;

		private readonly static Dictionary<string, string> supportedDocumentExtensionsAndMimeTypes = new Dictionary<string, string>
			{
				{"pdf", "application/pdf"},
				{"doc", "application/vnd.ms-word"},
				{"docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
				{"xls", "application/vnd.ms-excel"},
				{"xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
				{"txt", "text/plain"},
			};

		private readonly static List<string> supportedForPreviewDocumentExtensions = new List<string>() { "pdf", "txt" };
		private readonly static List<string> supportedImageExtensions = new List<string>() { "jpg", "jpeg", "png" };
		private static List<string> supportedDocumentExtensions => supportedDocumentExtensionsAndMimeTypes.Keys.ToList();

		private static IHostingEnvironment hostingEnvironment { get; set; }

		#endregion

		public FileManager(CompoundRepository compoundRepository, IHostingEnvironment hostingEnvironment, IStringLocalizer localizer)
		{
			this.compoundRepository = compoundRepository;
			FileManager.hostingEnvironment = hostingEnvironment;
			this.localizer = localizer;
		}

		public async Task UploadFileAsync(string objectEnglishName, IFormFile uploadedFile)
		{
			if (uploadedFile == null)
			{
				throw new Exception(localizer["PleaseSelectAFile"]);
			}

			var path = GetTargetPath(objectEnglishName, uploadedFile);
			if (File.Exists(path))
			{
				throw new Exception(localizer["AFileWithThisNameHasAlreadyBeenUploaded"]);
			}

			Directory.CreateDirectory(Path.GetDirectoryName(path));
			using var fileStream = new FileStream(path, FileMode.Create);
			await uploadedFile.CopyToAsync(fileStream);
		}

		private string GetTargetPath(string objectEnglishName, IFormFile uploadedFile)
		{
			var obj = compoundRepository.GetObjectByEnglishName(objectEnglishName);
			var uploadedFileExtension = GetExtensionWithoutDot(uploadedFile.FileName);
			if (supportedDocumentExtensions.Contains(uploadedFileExtension))
			{
				return hostingEnvironment.WebRootPath + obj.GetDocumentPath(uploadedFile.FileName);
			}
			else if (supportedImageExtensions.Contains(uploadedFileExtension))
			{
				var mainImageFullPath = hostingEnvironment.WebRootPath + obj.GetMainImagePath();
				return File.Exists(mainImageFullPath) ? GetNewSecondaryImagePath(mainImageFullPath, obj.Id) : mainImageFullPath;
			}
			else
			{
				throw new FormatException(localizer["UploadFileFormatIsNotSupported"].Value
					.Replace("{{supportedDocumentExtensions}}", String.Join(", ", supportedDocumentExtensions))
					.Replace("{{supportedImageExtensions}}", String.Join(", ", supportedImageExtensions)));
			}
		}

		public void DeleteFile(string deletedFileFullPath, string objectEnglishName)
		{
			if (File.Exists(deletedFileFullPath))
			{
				File.Delete(deletedFileFullPath);
			}

			var obj = compoundRepository.GetObjectByEnglishName(objectEnglishName);
			var mainImageFullPath = hostingEnvironment.WebRootPath + obj.GetMainImagePath();
			if (deletedFileFullPath == mainImageFullPath)
			{
				MakeSecondaryImageMain(obj, mainImageFullPath);
			}
		}

		private void MakeSecondaryImageMain(CommonObject obj, string mainImagePath)
		{
			var secondaryImagePath = GetFullImagePaths(obj).FirstOrDefault(p => p != mainImagePath);
			if (secondaryImagePath != null)
			{
				File.Move(secondaryImagePath, mainImagePath);
			}
		}

		public void MakeImageMain(string newMainImageFullPath, string objectEnglishName)
		{
			var obj = compoundRepository.GetObjectByEnglishName(objectEnglishName);
			var oldMainImageFullPath = hostingEnvironment.WebRootPath + obj.GetMainImagePath();
			if (newMainImageFullPath != oldMainImageFullPath)
			{
				File.Move(oldMainImageFullPath, GetNewSecondaryImagePath(oldMainImageFullPath, obj.Id));
				File.Move(newMainImageFullPath, oldMainImageFullPath);
			}
		}

		private string GetNewSecondaryImagePath(string path, int id) => path.Replace($"{id}.", $"{id}_{Guid.NewGuid()}.");

		#region Static methods

		public static bool IsTheMainImage(string imagePath, int objectId) => imagePath.Contains($@"\{objectId}.");

		public static string GetRelativePath(string fullPath) => fullPath.Substring(fullPath.IndexOf(@"\images"));

		public static string GetFileName(string fullPath)
		{
			var fileNameWithPrefix = Path.GetFileName(fullPath);
			var indexOfUnderscore = fileNameWithPrefix.IndexOf('_');
			return fileNameWithPrefix.Substring(indexOfUnderscore + 1);
		}

		public static List<string> GetFullImagePaths(CommonObject commonObject) => GetFullFilesPaths(commonObject)
			.Where(x => supportedImageExtensions.Contains(GetExtensionWithoutDot(x))).ToList();

		public static List<string> GetFullDocumentPaths(CommonObject commonObject) => GetFullFilesPaths(commonObject)
			.Where(x => supportedDocumentExtensions.Contains(GetExtensionWithoutDot(x))).ToList();

		public static List<string> GetFullFilesPaths(CommonObject commonObject)
		{
			var directoryPath = hostingEnvironment.WebRootPath + commonObject.GetDirectoryPath();
			if (!Directory.Exists(directoryPath))
			{
				return new List<string>();
			}

			var mainImage = Directory.GetFiles(directoryPath, commonObject.GetNamePatternOfMainImage());
			var otherFiles = Directory.GetFiles(directoryPath, commonObject.GetNamePatternOfAllFilesExceptMainImage());
			return mainImage.Union(otherFiles).ToList();
		}

		public static bool CanBePreviewed(string path)
		{
			var fileExtension = GetExtensionWithoutDot(path);
			return supportedForPreviewDocumentExtensions.Contains(fileExtension);
		}

		public static string GetContentType(string path)
		{
			var extension = GetExtensionWithoutDot(path);
			return supportedDocumentExtensionsAndMimeTypes[extension];
		}

		private static string GetExtensionWithoutDot(string path) => Path.GetExtension(path).ToLower().Replace(".", "");

		#endregion
	}
}
