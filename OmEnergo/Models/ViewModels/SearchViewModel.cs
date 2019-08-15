using System.Collections.Generic;

namespace OmEnergo.Models.ViewModels
{
	public class SearchViewModel
	{
		public string SearchString { get; set; }

		public IEnumerable<Section> Sections { get; set; }

		public IEnumerable<Product> Products { get; set; }

		public IEnumerable<ProductModel> ProductModels { get; set; }

		public SearchViewModel(string searchString, IEnumerable<Section> sections, IEnumerable<Product> products, IEnumerable<ProductModel> productModels)
		{
			SearchString = searchString;
			Sections = sections;
			Products = products;
			ProductModels = productModels;
		}
	}
}
