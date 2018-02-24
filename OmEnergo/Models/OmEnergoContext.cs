using Microsoft.EntityFrameworkCore;

namespace OmEnergo.Models
{
    public class OmEnergoContext : DbContext
    {
        public OmEnergoContext(DbContextOptions<OmEnergoContext> options) : base(options)
        {

        }
    }
}
