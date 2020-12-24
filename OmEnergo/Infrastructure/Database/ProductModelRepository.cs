using Microsoft.EntityFrameworkCore;
using OmEnergo.Models;
using System.Collections.Generic;
using System.Linq;

namespace OmEnergo.Infrastructure.Database
{
	public class ProductModelRepository : CommonObjectRepository<ProductModel>
	{
		public ProductModelRepository() : base() { }

		public ProductModelRepository(OmEnergoContext context) : base(context) { }

		public IEnumerable<ProductModel> GetProductModels(int sectionId) =>
			GetAll().Where(x => x.Section?.Id == sectionId || x.Product?.Section?.Id == sectionId);

		protected override IQueryable<ProductModel> GetAllQueryable() => db.ProductModels
			.Include(x => x.Section)
			.Include(x => x.Product).ThenInclude(x => x.Section);

		protected override IQueryable<ProductModel> GetAllSearchedItemsQueryable() => GetAllQueryable();
	}
}
