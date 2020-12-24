using Microsoft.EntityFrameworkCore;
using OmEnergo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Infrastructure.Database
{
	public class SectionRepository : CommonObjectRepository<Section>
	{
		public SectionRepository() : base() { }

		public SectionRepository(OmEnergoContext context) : base(context) { }

		public IEnumerable<Section> GetFullCatalog() => GetAllQueryable().Where(Section.IsMainSectionPredicate());

		public Task<List<Section>> GetOrderedMainSectionsAsync() => db.Sections.Include(x => x.ParentSection)
			.Where(Section.IsMainSectionPredicate())
			.OrderBy(x => x.SequenceNumber).ToListAsync();

		protected override IQueryable<Section> GetAllQueryable() => db.Sections
			.Include(x => x.Products).ThenInclude(x => x.Models)
			.Include(x => x.ChildSections).ThenInclude(x => x.Products).ThenInclude(x => x.Models)
			.Include(x => x.ChildSections).ThenInclude(x => x.ProductModels)
			.Include(x => x.ProductModels)
			.Include(x => x.ParentSection);

		protected override IQueryable<Section> GetAllSearchedItemsQueryable() => db.Sections;
	}
}
