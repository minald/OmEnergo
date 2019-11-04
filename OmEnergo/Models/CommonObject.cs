using Microsoft.AspNetCore.Mvc;
using OmEnergo.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace OmEnergo.Models
{
	public abstract class CommonObject : UniqueObject
	{
		[Display(Name = "Название")]
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
		public string Name { get; set; }

		[Display(Name = "Название для URL")]
		[Remote("IsNewEnglishName", "Admin", AdditionalFields = "Id", ErrorMessage = "Такое название для URL уже существует")]
		public string EnglishName { get; set; }

		[Display(Name = "Описание")]
		public string Description { get; set; }

		[Display(Name = "Порядковый номер")]
		public int SequenceNumber { get; set; }

        [Display(Name = "Метатег Title")]
        [StringLength(90)]
        public string MetatagTitle { get; set; }

        [Display(Name = "Метатег Description")]
        [StringLength(300)]
        public string MetatagDescription { get; set; }

        [Display(Name = "Метатег Keywords")]
        public string MetatagKeywords { get; set; }

        public abstract string GetDirectoryPath();

		public abstract string GetImageNamePrefix();

        public string GetMainImagePath() => GetFilePath(".jpg");

		public string GetMainImageThumbnailPath(int pixels) => GetFilePath($"-{pixels}.jpg");

		public string GetDocumentPath(string originalFilename) => GetFilePath($"_{originalFilename}");

        private string GetFilePath(string filenameLastPart)
        {
            string directoryPath = GetDirectoryPath();
            string imageNamePrefix = GetImageNamePrefix();
            string filename = $"{imageNamePrefix}{Id}{filenameLastPart}";
            return Path.Combine(directoryPath, filename);
        }

		public string GetNamePatternOfMainImage() => $"{GetImageNamePrefix()}{Id}.*";

		public string GetNamePatternOfAllFilesExceptMainImage() => $"{GetImageNamePrefix()}{Id}_*";

		public void SetEnglishNameIfEmpty()
		{
			if (String.IsNullOrEmpty(EnglishName))
			{
				EnglishName = Transliterator.FromRussianToEnglish(Name);
			}
		}
	}
}
