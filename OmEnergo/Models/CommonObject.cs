using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public abstract class CommonObject
    {
        public int Id { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Порядковый номер")]
        public int SequenceNumber { get; set; }

        public abstract string GetImageFullLink();
    }
}
