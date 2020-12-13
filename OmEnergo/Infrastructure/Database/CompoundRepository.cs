using OmEnergo.Models;
using System.Linq;

namespace OmEnergo.Infrastructure.Database
{
	public class CompoundRepository : Repository
	{
		private readonly SectionRepository sectionRepository;
		private readonly ProductRepository productRepository;
		private readonly ProductModelRepository productModelRepository;

		public CompoundRepository(SectionRepository sectionRepository, 
			ProductRepository productRepository, 
			ProductModelRepository productModelRepository)
		{
			this.sectionRepository = sectionRepository;
			this.productRepository = productRepository;
			this.productModelRepository = productModelRepository;
		}

		public void UpdateSectionAndSynchronizeProperties(Section section)
		{
			sectionRepository.Update(section);

			var products = productRepository.GetProducts(section.Id).ToList();
			products.ForEach(x => x.UpdateProperties(section.GetProductPropertyList()));
			productRepository.UpdateRange(products);

			var productModels = productModelRepository.GetProductModels(section.Id).ToList();
			productModels.ForEach(x => x.UpdateProperties(section.GetProductModelPropertyList()));
			productModelRepository.UpdateRange(productModels);
		}

		public CommonObject GetObjectByEnglishName(string englishName)
		{
			var section = sectionRepository.GetItemByEnglishName<Section>(englishName);
			if (section != null)
			{
				return section;
			}

			var product = productRepository.GetItemByEnglishName<Product>(englishName);
			if (product != null)
			{
				return product;
			}

			var productModel = productModelRepository.GetItemByEnglishName<ProductModel>(englishName);
			return productModel;
		}
	}
}
