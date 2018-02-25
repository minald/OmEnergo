using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class Booklet
    {
        [Key]
        public int Id { get; set; }

        public string Link { get; set; }
    }
}
