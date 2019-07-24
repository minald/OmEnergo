using Microsoft.AspNetCore.Mvc;
using OmEnergo.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace OmEnergo.Models
{
    public abstract class CommonObject : UniqueObject
    {
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Имя для URL")]
        [Remote("IsNewEnglishName", "Admin", ErrorMessage = "Такое имя для URL уже существует")]
        public string EnglishName { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Порядковый номер")]
        public int SequenceNumber { get; set; }

        public abstract string GetDirectoryPath();

        public string GetMainImageFullLink()
        {
            string directoryPath = GetDirectoryPath();
            string mainImageName = $"{Id}.jpg";
            return Path.Combine(directoryPath, mainImageName);
        }

        public string GetDocumentPath(string originalFilename)
        {
            string directoryPath = GetDirectoryPath();
            string filename = $"{Id}_{originalFilename}";
            return Path.Combine(directoryPath, filename);
        }

        public void SetEnglishNameIfEmpty()
        {
            if (String.IsNullOrEmpty(EnglishName))
            {
                EnglishName = Transliterator.FromRussianToEnglish(Name);
            }
        }
    }
}
