using Microsoft.EntityFrameworkCore;

namespace OmEnergo.Models
{
    public class OmEnergoContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public OmEnergoContext(DbContextOptions<OmEnergoContext> options) : base(options)
        {

        }
    }
}
