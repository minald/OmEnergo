using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class StabilizerModel
    {
        [Key]
        public int Id { get; set; }

        public Stabilizer Stabilizer { get; set; }

        public string Name { get; set; }

        public double? Price { get; set; }
    }
}
