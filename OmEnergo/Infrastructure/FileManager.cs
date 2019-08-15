using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

		private Repository Repository { get; set; }
		private static IHostingEnvironment HostingEnvironment { get; set; }

		private readonly static Dictionary<string, string> SupportedDocumentExtensionsAndMimeTypes = new Dictionary<string, string>
			{
				{"pdf", "application/pdf"},
				{"doc", "application/vnd.ms-word"},
				{"docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
				{"xls", "application/vnd.ms-excel"},
				{"xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
				{"txt", "text/plain"},
			};

		private readonly static List<string> SupportedForPreviewDocumentExtensions = new List<string>() { "pdf", "txt" };
		private readonly static List<string> SupportedImageExtensions = new List<string>() { "jpg", "jpeg", "png" };
		private static List<string> SupportedDocumentExtensions => SupportedDocumentExtensionsAndMimeTypes.Keys.ToList();

		#endregion

		public FileManager(Repository repository, IHostingEnvironment hostingEnvironment)
		{
			Repository = repository;
			HostingEnvironment = hostingEnvironment;
		}

		public async Task UploadFileAsync(string objectEnglishName, IFormFile uploadedFile)
		{
			if (uploadedFile == null)
			{
				throw new Exception("Пожалуйста, выберите файл");
			}
			
			string path = GetTargetPath(objectEnglishName, uploadedFile);
			if (File.Exists(path))
			{
				throw new Exception("Файл с таким именем уже загружен");
			}

			Directory.CreateDirectory(Path.GetDirectoryName(path));
			using (var fileStream = new FileStream(path, FileMode.Create))
			{
				await uploadedFile.CopyToAsync(fileStream);
			}
		}

		private string GetTargetPath(string objectEnglishName, IFormFile uploadedFile)
		{
			CommonObject obj = Repository.GetObjectByEnglishName(objectEnglishName);
			string uploadedFileExtension = GetExtensionWithoutDot(uploadedFile.FileName);
			if (SupportedDocumentExtensions.Contains(uploadedFileExtension))
			{
				return HostingEnvironment.WebRootPath + obj.GetDocumentPath(uploadedFile.FileName);
			}
			else if (SupportedImageExtensions.Contains(uploadedFileExtension))
			{
				string mainImageFullPath = HostingEnvironment.WebRootPath + obj.GetMainImagePath();
				return File.Exists(mainImageFullPath) ? GetNewSecondaryImagePath(mainImageFullPath, obj.Id) : mainImageFullPath;
			}
			else
			{
				throw new Exception("Формат загружаемого файла не поддерживается. "
					+ $"Поддерживаемые форматы документов: {String.Join(", ", SupportedDocumentExtensions)}. "
					+ $"Поддерживаемые форматы изображений: {String.Join(", ", SupportedImageExtensions)}.");
			}
		}

		public void DeleteFile(string deletedFileFullPath, string objectEnglishName)
		{
			if (File.Exists(deletedFileFullPath))
			{
				File.Delete(deletedFileFullPath);
			}

			CommonObject obj = Repository.GetObjectByEnglishName(objectEnglishName);
			string mainImageFullPath = HostingEnvironment.WebRootPath + obj.GetMainImagePath();
			if (deletedFileFullPath == mainImageFullPath)
			{
				MakeSecondaryImageMain(obj, mainImageFullPath);
			}
		}

		private void MakeSecondaryImageMain(CommonObject obj, string mainImagePath)
		{
			string secondaryImagePath = GetFullImagePaths(obj).FirstOrDefault(p => p != mainImagePath);
			if (secondaryImagePath != null)
			{
				File.Move(secondaryImagePath, mainImagePath);
			}
		}

		public void MakeImageMain(string newMainImageFullPath, string objectEnglishName)
		{
			CommonObject obj = Repository.GetObjectByEnglishName(objectEnglishName);
			string oldMainImageFullPath = HostingEnvironment.WebRootPath + obj.GetMainImagePath();
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
			string fileNameWithPrefix = Path.GetFileName(fullPath);
			int indexOfUnderscore = fileNameWithPrefix.IndexOf('_');
			return fileNameWithPrefix.Substring(indexOfUnderscore + 1);
		}

		public static List<string> GetFullImagePaths(CommonObject commonObject) => GetFullFilesPaths(commonObject)
			.Where(x => SupportedImageExtensions.Contains(GetExtensionWithoutDot(x))).ToList();

		public static List<string> GetFullDocumentPaths(CommonObject commonObject) => GetFullFilesPaths(commonObject)
			.Where(x => SupportedDocumentExtensions.Contains(GetExtensionWithoutDot(x))).ToList();

		public static List<string> GetFullFilesPaths(CommonObject commonObject)
		{
			string directoryPath = HostingEnvironment.WebRootPath + commonObject.GetDirectoryPath();
			if (!Directory.Exists(directoryPath))
			{
				return new List<string>();
			}

			string[] mainImage = Directory.GetFiles(directoryPath, commonObject.GetNamePatternOfMainImage());
			string[] otherFiles = Directory.GetFiles(directoryPath, commonObject.GetNamePatternOfAllFilesExceptMainImage());
			return mainImage.Union(otherFiles).ToList();
		}

		public static bool CanBePreviewed(string path)
		{
			string fileExtension = GetExtensionWithoutDot(path);
			return SupportedForPreviewDocumentExtensions.Contains(fileExtension);
		}

		public static string GetContentType(string path)
		{
			string extension = GetExtensionWithoutDot(path);
			return SupportedDocumentExtensionsAndMimeTypes[extension];
		}

		private static string GetExtensionWithoutDot(string path) => Path.GetExtension(path).ToLower().Replace(".", "");

		#endregion
	}
}
