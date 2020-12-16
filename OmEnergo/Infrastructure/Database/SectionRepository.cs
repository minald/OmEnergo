using Microsoft.EntityFrameworkCore;
using OmEnergo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Infrastructure.Database
{
	public class SectionRepository : Repository
	{
		public SectionRepository() : base() { }

		public SectionRepository(OmEnergoContext context) : base(context) { }

		public IEnumerable<Section> GetFullCatalog() => db.Sections
			.Include(x => x.ProductModels).Include(x => x.Products).ThenInclude(x => x.Models)
			.Where(Section.IsMainSectionPredicate());

		public async Task<List<Section>> GetOrderedMainSectionsAsync() => await db.Sections.Include(x => x.ParentSection)
			.Where(Section.IsMainSectionPredicate())
			.OrderBy(x => x.SequenceNumber).ToListAsync();

		protected override IQueryable<Section> GetAllQueryable<Section>() => (IQueryable<Section>)db.Sections
			.Include(x => x.Products).ThenInclude(x => x.Models)
			.Include(x => x.ChildSections).ThenInclude(x => x.Products)
			.Include(x => x.ChildSections).ThenInclude(x => x.ProductModels)
			.Include(x => x.ProductModels)
			.Include(x => x.ParentSection);

		protected override IQueryable<Section> GetAllSearchedItemsQueryable<Section>() => (IQueryable<Section>)db.Sections;
	}
}
