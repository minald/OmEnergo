using System.Collections.Generic;

namespace OmEnergo.Models.ViewModels
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
