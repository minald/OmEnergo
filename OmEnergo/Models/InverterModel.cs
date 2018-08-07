using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class InverterModel : ProductModel<Inverter>
    {
        [Display(Name = "Номинальная мощность")]
        public string NominalPower { get; set; }

        [Display(Name = "Пиковая мощность")]
        public string PeakPower { get; set; }

        [Display(Name = "Напряжение батареи")]
        public string BatteryVoltage { get; set; }
    }
}
