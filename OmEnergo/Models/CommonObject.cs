using Microsoft.AspNetCore.Mvc;
using OmEnergo.Infrastructure;
using OmEnergo.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace OmEnergo.Models
{
	public abstract class CommonObject : UniqueObject
	{
		[Display(Name = "Name", ResourceType = typeof(SharedResource))]
		[Required(ErrorMessageResourceName = "PleaseFillInTheField", ErrorMessageResourceType = typeof(SharedResource))]
		public string Name { get; set; }

		[Display(Name = "NameForUrl", ResourceType = typeof(SharedResource))]
		[Remote("IsNewEnglishName", "Admin", AdditionalFields = "Id",
			ErrorMessageResourceName = "SuchNameForUrlAlreadyExists", ErrorMessageResourceType = typeof(SharedResource))]
		public string EnglishName { get; set; }

		[Display(Name = "Description", ResourceType = typeof(SharedResource))]
		public string Description { get; set; }

		[Display(Name = "SequenceNumber", ResourceType = typeof(SharedResource))]
		public int SequenceNumber { get; set; }

		[Display(Name = "MetatagTitle", ResourceType = typeof(SharedResource))]
		[StringLength(90)]
		public string MetatagTitle { get; set; }

		[Display(Name = "MetatagDescription", ResourceType = typeof(SharedResource))]
		[StringLength(300)]
		public string MetatagDescription { get; set; }

		[Display(Name = "MetatagKeywords", ResourceType = typeof(SharedResource))]
		public string MetatagKeywords { get; set; }

		public abstract string GetDirectoryPath();

		public abstract string GetImageNamePrefix();

		public string GetMainImagePath() => GetFilePath(".jpg");

		public string GetMainImageThumbnailPath(int pixels) => GetFilePath($"-{pixels}.jpg");

		public string GetDocumentPath(string originalFilename) => GetFilePath($"_{originalFilename}");

		private string GetFilePath(string filenameLastPart)
		{
			var directoryPath = GetDirectoryPath();
			var imageNamePrefix = GetImageNamePrefix();
			var filename = $"{imageNamePrefix}{Id}{filenameLastPart}";
			return Path.Combine(directoryPath, filename);
		}

		public string GetNamePatternOfMainImage() => $"{GetImageNamePrefix()}{Id}.*";

		public string GetNamePatternOfAllFilesExceptMainImage() => $"{GetImageNamePrefix()}{Id}_*";

		public void SetEnglishNameIfItsEmpty()
		{
			if (String.IsNullOrEmpty(EnglishName))
			{
				EnglishName = Transliterator.FromRussianToEnglish(Name);
			}
		}
	}
}
