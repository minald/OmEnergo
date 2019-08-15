using Microsoft.EntityFrameworkCore;
using OmEnergo.Models;

namespace OmEnergo.Infrastructure.Database
{
	public class OmEnergoContext : DbContext
	{
		public DbSet<Section> Sections { get; set; }

		public DbSet<Product> Products { get; set; }

		public DbSet<ProductModel> ProductModels { get; set; }

		public DbSet<ConfigKey> ConfigKeys { get; set; }

		public OmEnergoContext() { }

		public OmEnergoContext(DbContextOptions<OmEnergoContext> options) : base(options) { }
	}
}
