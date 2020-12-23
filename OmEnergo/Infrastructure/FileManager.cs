using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OmEnergo.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Infrastructure
{
	public class FileManager
	{
		public readonly static Dictionary<string, string> SupportedDocumentExtensionsAndMimeTypes = new Dictionary<string, string>
			{
				{"pdf", "application/pdf"},
				{"doc", "application/vnd.ms-word"},
				{"docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
				{"xls", "application/vnd.ms-excel"},
				{"xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
				{"txt", "text/plain"}
			};

		private static string webRootPath;

		private readonly IStringLocalizer localizer;

		public FileManager(string webRootPath, IStringLocalizer localizer)
		{
			FileManager.webRootPath = webRootPath;
			this.localizer = localizer;
		}

		public static string GetContentType(string path)
		{
			var extension = GetExtensionWithoutDot(path);
			return SupportedDocumentExtensionsAndMimeTypes[extension];
		}

		public static string MakeFullPathFromRelative(string relativePath) => webRootPath + relativePath;

		public static string GetRelativePath(string fullPath)
		{
			var imagesFolderIndex = fullPath.IndexOf(@"\images");
			if (imagesFolderIndex == -1)
			{
				imagesFolderIndex = 0;
			}

			return fullPath.Substring(imagesFolderIndex);
		}

		public static string GetFileName(string fullPath)
		{
			var fileNameWithPrefix = Path.GetFileName(fullPath);
			var indexOfUnderscore = fileNameWithPrefix.IndexOf('_');
			return fileNameWithPrefix.Substring(indexOfUnderscore + 1);
		}

		public static List<string> GetFullFilesPathsByNamePatterns(string directoryPath, List<string> namePatterns)
		{
			if (!Directory.Exists(directoryPath))
			{
				return new List<string>();
			}

			var resultPaths = new List<string>();
			namePatterns.ForEach(p => resultPaths = resultPaths.Union(Directory.GetFiles(directoryPath, p)).ToList());
			return resultPaths;
		}

		public static string GetExtensionWithoutDot(string path) => Path.GetExtension(path).ToLower().Replace(".", "");

		public async Task UploadFileAsync(string path, IFormFile uploadedFile)
		{
			if (uploadedFile == null)
			{
				throw new ArgumentNullException(localizer[nameof(SharedResource.PleaseSelectAFile)]);
			}

			if (File.Exists(path))
			{
				throw new ArgumentException(localizer[nameof(SharedResource.AFileWithThisNameHasAlreadyBeenUploaded)]);
			}

			Directory.CreateDirectory(Path.GetDirectoryName(path));
			using var fileStream = new FileStream(path, FileMode.Create);
			await uploadedFile.CopyToAsync(fileStream);
		}

		public void DeleteFile(string deletedFileFullPath)
		{
			if (File.Exists(deletedFileFullPath))
			{
				File.Delete(deletedFileFullPath);
			}
		}

		public void RenameFile(string sourcePath, string destinationPath)
		{
			if (sourcePath != null)
			{
				File.Move(sourcePath, destinationPath);
			}
		}
	}
}
