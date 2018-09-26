using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Models
{
    public class SearchViewModel
    {
		public IEnumerable<Section> Sections { get; set; }

		public IEnumerable<Product> Products { get; set; }

		public IEnumerable<ProductModel> ProductModels { get; set; }

		public SearchViewModel(IEnumerable<Section> sections, IEnumerable<Product> products, IEnumerable<ProductModel> productModels)
		{
			Sections = sections;
			Products = products;
			ProductModels = productModels;
		}
	}
}
