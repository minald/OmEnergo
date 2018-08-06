using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class Switch
    {
        [Key]
        public int Id { get; set; }

        public Product Product { get; set; }

        public List<SwitchModel> Models { get; set; }

        public string Series { get; set; } //Enum in the future

        public string MainImageLink { get; set; }

        public string Description { get; set; }

        [Display(Name = "Номинальное напряжение")]
        public string NominalVoltage { get; set; }

        [Display(Name = "Максимальный ток")]
        public string MaximalAmperage { get; set; }

        [Display(Name = "Степень защиты")]
        public string ProtectionDegree { get; set; }

        [Display(Name = "Диапазон рабочих температур")]
        public string WorkingTemperatureRange { get; set; }

        public string GetImageFullLink() => $"/images/{Product.EnglishName}/{Series.Replace('"', '\'')}/{MainImageLink}";
    }
}
