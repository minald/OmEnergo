using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Models
{
    public class SearchViewModel
    {
		public IEnumerable<Section> Sections { get; set; }

		public IEnumerable<CommonProduct> Products { get; set; }

		public IEnumerable<CommonProductModel> ProductModels { get; set; }

		public SearchViewModel(IEnumerable<Section> sections, IEnumerable<CommonProduct> products, IEnumerable<CommonProductModel> productModels)
		{
			Sections = sections;
			Products = products;
			ProductModels = productModels;
		}
	}
}
