using Microsoft.EntityFrameworkCore;
using OmEnergo.Models;
using System.Collections.Generic;
using System.Linq;

namespace OmEnergo.Infrastructure.Database
{
	public class ProductModelRepository : Repository, ISearchableRepository<ProductModel>
	{
		public ProductModelRepository() : base() { }

		public ProductModelRepository(OmEnergoContext context) : base(context) { }

		protected override IQueryable<ProductModel> GetAllQueryable<ProductModel>() => (IQueryable<ProductModel>)db.ProductModels
			.Include(x => x.Section)
			.Include(x => x.Product).ThenInclude(x => x.Section);

		public IEnumerable<ProductModel> GetSearchedItems(string searchString) =>
			GetAllQueryable<ProductModel>().Where(x => x.Name.Contains(searchString));

		public IEnumerable<ProductModel> GetProductModels(int sectionId) =>
			GetAll<ProductModel>().Where(x => x.Section?.Id == sectionId || x.Product?.Section?.Id == sectionId);
	}
}
