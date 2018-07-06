using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class Autotransformer
    {
        [Key]
        public int Id { get; set; }

        public Product Product { get; set; }

        public string Series { get; set; } //Enum in the future

        public string MainImageLink { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        [Display(Name = "Диапазон входного напряжения")]
        public string InputVoltageRange { get; set; } //Class in the future, e.g. ("140 - 260 B")

        [Display(Name = "Диапазон выходного напряжения")]
        public string OutputVoltageRange { get; set; } //Class in the future, e.g. ("140 - 260 B")

        [Display(Name = "Количество фаз")]
        public int PhasesAmount { get; set; }

        [Display(Name = "Частота сети")]
        public string NetworkFrequency { get; set; } //Class in the future, e.g. ("50 (60) Гц") 

        public List<AutotransformerModel> Models { get; set; }

        public string GetImageFullLink() => $"/images/{Product.EnglishName}/{Series.Replace('"', '\'')}/{MainImageLink}";

    }
}
