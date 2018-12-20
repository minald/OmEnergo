using OmEnergo.Infrastucture;
using System;
using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public abstract class CommonObject
    {
        public int Id { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Имя для URL")]
        public string EnglishName { get; set; }

        [Display(Name = "Порядковый номер")]
        public int SequenceNumber { get; set; }

        public abstract string GetImageFullLink();

        public void SetEnglishNameIfEmpty()
        {
            if (String.IsNullOrEmpty(EnglishName))
            {
                EnglishName = Transliterator.FromRussianToEnglish(Name);
            }
        }
    }
}
