using Microsoft.AspNetCore.Mvc;
using OmEnergo.Infrastructure.Database;
using System.Collections.Generic;

namespace OmEnergo.Models
{
	public class NavigationTreeSidebar : ViewComponent
	{
		private IEnumerable<Section> sections { get; set; }

		public NavigationTreeSidebar(Repository repository) => sections = repository.GetFullCatalog();

		public IViewComponentResult Invoke() => View(sections);
	}
}
