using Microsoft.EntityFrameworkCore;
using OmEnergo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OmEnergo.Infrastructure.Database
{
	public class SectionRepository : Repository, ISearchableRepository<Section>
	{
		public SectionRepository() : base() { }

		public SectionRepository(OmEnergoContext context) : base(context) { }

		protected override IQueryable<Section> GetAllQueryable<Section>() => (IQueryable<Section>)db.Sections
			.Include(x => x.Products).ThenInclude(x => x.Models)
			.Include(x => x.ChildSections).ThenInclude(x => x.Products)
			.Include(x => x.ChildSections).ThenInclude(x => x.ProductModels)
			.Include(x => x.ProductModels)
			.Include(x => x.ParentSection);

		public IEnumerable<Section> GetSearchedItems(string searchString) =>
			db.Sections.Where(x => x.Name.Contains(searchString));

		public IEnumerable<Section> GetOrderedMainSections() => db.Sections.Include(x => x.ParentSection)
			.Where(Section.IsMainSectionPredicate())
			.OrderBy(x => x.SequenceNumber);

		public IEnumerable<Section> GetFullCatalog() => db.Sections
			.Include(x => x.ProductModels).Include(x => x.Products).ThenInclude(x => x.Models)
			.Where(Section.IsMainSectionPredicate());
	}
}
