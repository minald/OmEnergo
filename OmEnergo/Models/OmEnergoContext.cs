using Microsoft.EntityFrameworkCore;

namespace OmEnergo.Models
{
    public class OmEnergoContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Stabilizer> Stabilizers { get; set; }

        public DbSet<StabilizerModel> StabilizerModels { get; set; }

        public OmEnergoContext() { }

        public OmEnergoContext(DbContextOptions<OmEnergoContext> options) : base(options) {  }
    }
}
