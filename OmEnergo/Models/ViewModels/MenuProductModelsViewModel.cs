using System.Collections.Generic;

namespace OmEnergo.Models.ViewModels
{
    public class MenuProductModelsViewModel
    {
		public List<ProductModel> ProductModels { get; set; }

		public string SectionName { get; set; } 

		public MenuProductModelsViewModel(List<ProductModel> productModels, string sectionName)
		{
			ProductModels = productModels;
			SectionName = sectionName;
		}
    }
}
