using Microsoft.AspNetCore.Mvc;
using OmEnergo.Infrastructure.Database;
using System.Collections.Generic;

namespace OmEnergo.Models
{
	public class NavigationTreeSidebar : ViewComponent
	{
		private readonly IEnumerable<Section> sections;

		public NavigationTreeSidebar(SectionRepository sectionRepository) => sections = sectionRepository.GetFullCatalog();

		public IViewComponentResult Invoke() => View(sections);
	}
}
