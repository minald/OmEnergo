using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class Autotransformer //Лабораторный автотрансформатор
    {
        [Key]
        public int Id { get; set; }

        public Section Section { get; set; }

        public List<AutotransformerModel> Models { get; set; }

        public string Series { get; set; } //Enum in the future

        public string MainImageLink { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        [Display(Name = "Входное напряжение")]
        public string InputVoltage { get; set; } //Class in the future, e.g. ("140 - 260 B")

        [Display(Name = "Диапазон выходного напряжения")]
        public string OutputVoltageRange { get; set; } //Class in the future, e.g. ("140 - 260 B")

        [Display(Name = "Количество фаз")]
        public int PhasesAmount { get; set; }

        [Display(Name = "Частота сети")]
        public string NetworkFrequency { get; set; } //Class in the future, e.g. ("50 (60) Гц") 

        public string GetImageFullLink() => $"/images/{Section.EnglishName}/{Series.Replace('"', '\'')}/{MainImageLink}";
    }
}
