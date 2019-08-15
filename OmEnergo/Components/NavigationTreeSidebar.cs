using Microsoft.AspNetCore.Mvc;
using OmEnergo.Infrastructure.Database;
using System.Collections.Generic;

namespace OmEnergo.Models
{
	public class NavigationTreeSidebar : ViewComponent
	{
		private IEnumerable<Section> Sections { get; set; }

		public NavigationTreeSidebar(Repository repository) => Sections = repository.GetFullCatalog();

		public IViewComponentResult Invoke() => View(Sections);
	}
}
