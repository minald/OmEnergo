﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class Inverter
    {
        [Key]
        public int Id { get; set; }

        public Product Product { get; set; }

        public string Series { get; set; } //Enum in the future

        public string MainImageLink { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        [Display(Name = "Диапазон выходного напряжения")]
        public string OutputVoltageRange { get; set; } //Class in the future, e.g. ("140 - 260 B")

        [Display(Name = "Количество фаз")]
        public int PhasesAmount { get; set; }

        [Display(Name = "Частота выходного напряжения")]
        public string FrequencyOfOutputVoltage { get; set; } //Class in the future, e.g. ("50 (60) Гц") 

        [Display(Name = "КПД")]
        public string Efficiency { get; set; } //Int in the future, e.g. ("98%") 

        [Display(Name = "Время переключения")]
        public string SwitchingTime { get; set; } //Int in the future, e.g. ("4 мс") 

        [Display(Name = "Индикация")]
        public string Indication { get; set; } //List<Enum> in the future 

        [Display(Name = "Способ охлаждения")]
        public string CoolingMethod { get; set; }

        [Display(Name = "Рабочая температура")]
        public string WorkingTemperature { get; set; } //Class in the future, e.g. ("-20 - +40°С")

        public List<InverterModel> Models { get; set; }

        public string GetImageFullLink() => $"/images/{Product.EnglishName}/{Series.Replace('"', '\'')}/{MainImageLink}";
    }
}