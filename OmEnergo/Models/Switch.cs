using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    //Переключатель или выключатель
    public class Switch : Product
    {
        public List<SwitchModel> Models { get; set; }

        public string Description { get; set; }

        [Display(Name = "Номинальное напряжение")]
        public string NominalVoltage { get; set; }

        [Display(Name = "Максимальный ток")]
        public string MaximalAmperage { get; set; }

        [Display(Name = "Степень защиты")]
        public string ProtectionDegree { get; set; }

        [Display(Name = "Диапазон рабочих температур")]
        public string WorkingTemperatureRange { get; set; }
    }
}
