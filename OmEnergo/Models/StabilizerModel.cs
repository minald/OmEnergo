using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class StabilizerModel : ProductModel<Stabilizer>
    {
        [Display(Name = "Номинальная мощность")]
        public string NominalPower { get; set; }

        [Display(Name = "Подключение в сеть")]
        public string NetworkConnection { get; set; }

        [Display(Name = "Подключение нагрузки")]
        public string LoadConnection { get; set; }
    }
}
