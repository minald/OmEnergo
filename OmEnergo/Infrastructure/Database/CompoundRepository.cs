using OmEnergo.Models;
using System.Linq;
using System.Threading.Tasks;

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

		public async Task UpdateSectionAndSynchronizePropertiesAsync(Section section)
		{
			await sectionRepository.UpdateAsync(section);

			var products = await productRepository.GetProducts(section.Id);
			products.ForEach(x => x.UpdateProperties(section.GetProductPropertyList()));
			await productRepository.UpdateRangeAsync(products);

			var productModels = productModelRepository.GetProductModels(section.Id).ToList();
			productModels.ForEach(x => x.UpdateProperties(section.GetProductModelPropertyList()));
			await productModelRepository.UpdateRangeAsync(productModels);
		}

		public async Task<CommonObject> GetObjectByEnglishNameAsync(string englishName)
		{
			var section = await sectionRepository.GetItemByEnglishNameAsync<Section>(englishName);
			if (section != null)
			{
				return section;
			}

			var product = await productRepository.GetItemByEnglishNameAsync<Product>(englishName);
			if (product != null)
			{
				return product;
			}

			var productModel = await productModelRepository.GetItemByEnglishNameAsync<ProductModel>(englishName);
			return productModel;
		}
	}
}
