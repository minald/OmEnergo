using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Models
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
