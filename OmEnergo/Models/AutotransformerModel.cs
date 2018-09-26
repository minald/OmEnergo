using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class AutotransformerModel : OldProductModel<Autotransformer>
    {
        [Display(Name = "Номинальная мощность")]
        public string NominalPower { get; set; }

        [Display(Name = "Максимальный рабочий ток")]
        public string MaximalWorkingAmperage { get; set; }
    }
}
