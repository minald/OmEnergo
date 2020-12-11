using OmEnergo.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
	public class ProductModel : ProductObject
	{
		public Section Section { get; set; }

		public Product Product { get; set; }

		[Display(Name = "Price", ResourceType = typeof(SharedResource))]
		public double Price { get; set; }

		public ProductModel() { }

		public ProductModel(Section section, Product product)
		{
			Section = section;
			Product = product;
			UpdateProperties((section ?? product.Section).GetProductModelPropertyList());
		}

		public override string GetDirectoryPath()
		{
			var parentPath = Section != null ? $"{Section?.Id}" : $@"{Product.Section.Id}\{Product.Id}";
			return $@"\images\{parentPath}";
		}

		public override string GetImageNamePrefix() => "m";

		public string GetStringPriceFractionalPart()
		{
			var prefix = GetPriceFractionalPart() < 10 ? "0" : "";
			return prefix + GetPriceFractionalPart().ToString();
		}

		private int GetPriceFractionalPart() => (int)Math.Round((Price - GetPriceIntegerPart()) * 100);

		public int GetPriceIntegerPart() => (int)Price;
	}
}
