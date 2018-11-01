using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Models
{
    public class TreeViewMenu : ViewComponent
    {
		List<Section> sections;
		Repository Repository;

		public TreeViewMenu(OmEnergoContext context)
		{
			Repository = new Repository(context);
			sections = Repository.GetFullCatalog().ToList();
		}

		public IViewComponentResult Invoke()
		{
			return View(sections);
		}
    }
}
