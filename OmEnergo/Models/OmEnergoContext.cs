using Microsoft.EntityFrameworkCore;

namespace OmEnergo.Models
{
    public class OmEnergoContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Stabilizer> Stabilizers { get; set; }

        public DbSet<Booklet> Booklets { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        public OmEnergoContext(DbContextOptions<OmEnergoContext> options) : base(options) {  }
    }
}
