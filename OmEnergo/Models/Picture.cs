using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class Picture
    {
        [Key]
        public int Id { get; set; }

        public string Link { get; set; }
    }
}
