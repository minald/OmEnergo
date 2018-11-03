using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace OmEnergo.Models
{
    public class TreeViewMenu : ViewComponent
    {
		public List<Section> Sections;
        public Repository Repository;

		public TreeViewMenu(OmEnergoContext context)
		{
			Repository = new Repository(context);
			Sections = Repository.GetFullCatalog().ToList();
		}

		public IViewComponentResult Invoke() => View(Sections);
    }
}
