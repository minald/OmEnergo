using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace OmEnergo.Models
{
    public class TreeViewMenu : ViewComponent
    {
		public List<Section> Sections { get; set; }
        public Repository Repository { get; set; }

        public TreeViewMenu(Repository repository)
		{
			Repository = repository;
			Sections = Repository.GetFullCatalog().ToList();
		}

		public IViewComponentResult Invoke() => View(Sections);
    }
}
