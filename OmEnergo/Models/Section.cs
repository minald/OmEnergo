using OmEnergo.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OmEnergo.Models
{
	public class Section : CommonObject
	{
		public Section ParentSection { get; set; }

		public List<Section> ChildSections { get; set; }

		public List<Product> Products { get; set; }

		public List<ProductModel> ProductModels { get; set; }

		[Display(Name = "ProductProperties", ResourceType = typeof(SharedResource))]
		public string ProductProperties { get; set; } //Semicolon-separated array

		[Display(Name = "ProductModelProperties", ResourceType = typeof(SharedResource))]
		public string ProductModelProperties { get; set; } //Semicolon-separated array

		public Section() {}

		public override string GetDirectoryPath() => @"\images";

		public override string GetImageNamePrefix() => "s";

		public IEnumerable<CommonObject> GetOrderedNestedObjects()
		{
			var list = new List<CommonObject>();
			list.AddRange(ChildSections ?? new List<Section>());
			list.AddRange(Products ?? new List<Product>());
			list.AddRange(ProductModels ?? new List<ProductModel>());
			return list.OrderBy(x => x.SequenceNumber);
		}

		public List<string> GetProductPropertyList() => ProductProperties?.Split(';')?.ToList() ?? new List<string>();

		public List<string> GetProductModelPropertyList() => ProductModelProperties?.Split(';')?.ToList() ?? new List<string>();

		public bool IsMainSection() => ParentSection == null;

		public bool HasChildSections() => ChildSections.Count != 0;
	}
}
