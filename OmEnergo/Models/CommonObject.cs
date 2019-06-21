using OmEnergo.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace OmEnergo.Models
{
    public abstract class CommonObject
    {
        public int Id { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Имя для URL")]
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

        public void SetEnglishNameIfEmpty()
        {
            if (String.IsNullOrEmpty(EnglishName))
            {
                EnglishName = Transliterator.FromRussianToEnglish(Name);
            }
        }
    }
}
