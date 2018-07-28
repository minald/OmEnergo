﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class Stabilizer
    {
        [Key]
        public int Id { get; set; }

        public Product Product { get; set; }

        public string Series { get; set; } //Enum in the future

        public string MainImageLink { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        [Display(Name = "Тип стабилизации")]
        public string StabilizationType { get; set; } //Enum in the future 

        [Display(Name = "Точность стабилизации")]
        public string StabilizationAccuracy { get; set; } //Class in the future, e.g. ("220B +- 5%") 

        [Display(Name = "Количество фаз")]
        public int? PhasesAmount { get; set; }

        [Obsolete]
        [Display(Name = "Рабочая частота сети")]
        public string OperatingFrequencyOfNetwork { get; set; } //Class in the future, e.g. ("50 (60) Гц") 

        [Display(Name = "Диапазон входного напряжения")]
        public string InputVoltageRange { get; set; } //Class in the future, e.g. ("140 - 260 B")

        [Display(Name = "Допустимая длительная перегрузка")]
        public string AllowableDurableOverload { get; set; } //Int in the future, e.g. ("<= 110%") 

        [Display(Name = "Тип установки")]
        public string InstallationType { get; set; } //Int in the future, e.g. ("98%") 

        [Obsolete]
        [Display(Name = "КПД")]
        public string Efficiency { get; set; } //Int in the future, e.g. ("98%") 

        [Display(Name = "Время переключения")]
        public string SwitchingTime { get; set; } //Int in the future, e.g. ("4 мс") 

        [Display(Name = "Индикация")]
        public string Indication { get; set; } //List<Enum> in the future 

        [Display(Name = "Рабочая температура")]
        public string WorkingTemperature { get; set; } //Class in the future, e.g. ("-20 - +40°С")

        [Display(Name = "Реализованные защиты")]
        public string ImplementedProtections { get; set; } //Class in the future, e.g. ("-20 - +40°С")

        [Obsolete("Use ImplementedProtections instead")]
        [Display(Name = "Защита от короткого замыкания")]
        public bool? ShortCircuitProtection { get; set; }

        [Obsolete("Use ImplementedProtections instead")]
        [Display(Name = "Защита от скачков напряжения")]
        public bool? VoltageSurgesProtection { get; set; }

        [Obsolete("Use ImplementedProtections instead")]
        [Display(Name = "Регулируемая задержка")]
        public bool? AdjustableDelay { get; set; }

        [Display(Name = "Режим \"Байпас\"")]
        public string BypassMode { get; set; }

        public List<StabilizerModel> Models { get; set; }

        public string GetImageFullLink() => $"/images/{Product.EnglishName}/{Series.Replace('"', '\'')}/{MainImageLink}";
    }
}
