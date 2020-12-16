using Microsoft.EntityFrameworkCore;
using OmEnergo.Models;
using System.Collections.Generic;
using System.Linq;

namespace OmEnergo.Infrastructure.Database
{
	public class ProductModelRepository : Repository
	{
		public ProductModelRepository() : base() { }

		public ProductModelRepository(OmEnergoContext context) : base(context) { }

		public IEnumerable<ProductModel> GetProductModels(int sectionId) =>
			GetAll<ProductModel>().Where(x => x.Section?.Id == sectionId || x.Product?.Section?.Id == sectionId);

		protected override IQueryable<ProductModel> GetAllQueryable<ProductModel>() => (IQueryable<ProductModel>)db.ProductModels
			.Include(x => x.Section)
			.Include(x => x.Product).ThenInclude(x => x.Section);

		protected override IQueryable<ProductModel> GetAllSearchedItemsQueryable<ProductModel>() => GetAllQueryable<ProductModel>();
	}
}
