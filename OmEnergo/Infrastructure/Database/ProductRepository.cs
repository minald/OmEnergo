using Microsoft.EntityFrameworkCore;
using OmEnergo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Infrastructure.Database
{
	public class ProductRepository : CommonObjectRepository<Product>
	{
		public ProductRepository() : base() { }

		public ProductRepository(OmEnergoContext context) : base(context) { }

		public Task<List<Product>> GetProducts(int sectionId) => GetAllQueryable()
			.Where(x => x.Section.Id == sectionId).ToListAsync();

		protected override IQueryable<Product> GetAllQueryable() => db.Products
			.Include(x => x.Section).Include(x => x.Models);

		protected override IQueryable<Product> GetAllSearchedItemsQueryable() => db.Products.Include(x => x.Section);
	}
}
