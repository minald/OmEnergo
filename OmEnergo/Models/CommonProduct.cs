namespace OmEnergo.Models
{
    public class CommonProduct
    {
        public int Id { get; set; }

        public Section Section { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Properties { get; set; }

        public string MainImageLink { get; set; }
    }
}
