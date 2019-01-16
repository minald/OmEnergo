using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace OmEnergo.Models
{
    public class TreeViewMenu : ViewComponent
    {
		private IEnumerable<Section> Sections { get; set; }

        public TreeViewMenu(Repository repository) => Sections = repository.GetFullCatalog();

        public IViewComponentResult Invoke() => View(Sections);
    }
}
