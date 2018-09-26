using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    //Лабораторный автотрансформатор
    public class Autotransformer : OldProduct 
    {
        public List<AutotransformerModel> Models { get; set; }

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
    }
}
