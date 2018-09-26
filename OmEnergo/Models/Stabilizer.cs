using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    //Стабилизатор
    public class Stabilizer : OldProduct
    {
        public List<StabilizerModel> Models { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        [Display(Name = "Тип стабилизации")]
        public string StabilizationType { get; set; } //Enum in the future 

        [Display(Name = "Точность стабилизации")]
        public string StabilizationAccuracy { get; set; } //Class in the future, e.g. ("220B +- 5%") 

        [Display(Name = "Количество фаз")]
        public int? PhasesAmount { get; set; }

        [Display(Name = "Диапазон входного напряжения")]
        public string InputVoltageRange { get; set; } //Class in the future, e.g. ("140 - 260 B")

        [Display(Name = "Допустимая длительная перегрузка")]
        public string AllowableDurableOverload { get; set; } //Int in the future, e.g. ("<= 110%") 

        [Display(Name = "Тип установки")]
        public string InstallationType { get; set; } //Int in the future, e.g. ("98%") 

        [Display(Name = "Время переключения")]
        public string SwitchingTime { get; set; } //Int in the future, e.g. ("4 мс") 

        [Display(Name = "Индикация")]
        public string Indication { get; set; } //List<Enum> in the future 

        [Display(Name = "Рабочая температура")]
        public string WorkingTemperature { get; set; } //Class in the future, e.g. ("-20 - +40°С")

        [Display(Name = "Реализованные защиты")]
        public string ImplementedProtections { get; set; } //Class in the future, e.g. ("-20 - +40°С")

        [Display(Name = "Регулируемая задержка")]
        public bool? AdjustableDelay { get; set; }

        [Display(Name = "Режим \"Байпас\"")]
        public string BypassMode { get; set; }

        [Obsolete]
        [Display(Name = "Рабочая частота сети")]
        public string OperatingFrequencyOfNetwork { get; set; }

        [Obsolete]
        [Display(Name = "КПД")]
        public string Efficiency { get; set; }

        [Obsolete("Use ImplementedProtections instead")]
        [Display(Name = "Защита от короткого замыкания")]
        public bool? ShortCircuitProtection { get; set; }

        [Obsolete("Use ImplementedProtections instead")]
        [Display(Name = "Защита от скачков напряжения")]
        public bool? VoltageSurgesProtection { get; set; }
    }
}
