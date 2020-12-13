using Microsoft.EntityFrameworkCore;
using OmEnergo.Models;
using System.Collections.Generic;
using System.Linq;

namespace OmEnergo.Infrastructure.Database
{
	public class ProductRepository : Repository, ISearchableRepository<Product>
	{
		public ProductRepository() : base() { }

		public ProductRepository(OmEnergoContext context) : base(context) { }

		protected override IQueryable<Product> GetAllQueryable<Product>() => (IQueryable<Product>)db.Products
			.Include(x => x.Section).Include(x => x.Models);

		public IEnumerable<Product> GetSearchedItems(string searchString) =>
			db.Products.Include(x => x.Section).Where(x => x.Name.Contains(searchString));

		public IEnumerable<Product> GetProducts(int sectionId) => GetAllQueryable<Product>().Where(x => x.Section.Id == sectionId);
	}
}
