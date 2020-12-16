using Microsoft.EntityFrameworkCore;
using OmEnergo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Infrastructure.Database
{
	public class ProductRepository : Repository
	{
		public ProductRepository() : base() { }

		public ProductRepository(OmEnergoContext context) : base(context) { }

		public async Task<List<Product>> GetProducts(int sectionId) => await GetAllQueryable<Product>().Where(x => x.Section.Id == sectionId).ToListAsync();

		protected override IQueryable<Product> GetAllQueryable<Product>() => (IQueryable<Product>)db.Products
			.Include(x => x.Section).Include(x => x.Models);

		protected override IQueryable<Product> GetAllSearchedItemsQueryable<Product>() => 
			(IQueryable<Product>)db.Products.Include(x => x.Section);
	}
}
