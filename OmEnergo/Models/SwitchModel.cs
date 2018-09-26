using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class SwitchModel : OldProductModel<Switch>
    {
        [Display(Name = "Доступная длина провода")]
        public double AvailableWireLength { get; set; }

        [Display(Name = "Максимальный ток")]
        public string MaximalAmperage { get; set; }
    }
}
