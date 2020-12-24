using OmEnergo.Infrastructure.Database;
using OmEnergo.Models;
using OmEnergo.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Services
{
	public class RepositoryService
	{
		private readonly SectionRepository sectionRepository;
		private readonly ProductRepository productRepository;
		private readonly ProductModelRepository productModelRepository;
		private readonly ConfigKeyRepository configKeyRepository;

		public RepositoryService(SectionRepository sectionRepository,
			ProductRepository productRepository,
			ProductModelRepository productModelRepository,
			ConfigKeyRepository configKeyRepository)
		{
			this.sectionRepository = sectionRepository;
			this.productRepository = productRepository;
			this.productModelRepository = productModelRepository;
			this.configKeyRepository = configKeyRepository;
		}

		public List<ConfigKey> GetAllConfigKeys() => configKeyRepository.GetAll().ToList();

		public async Task UpdateConfigKeysAsync(List<ConfigKey> configKeys) => await configKeyRepository.UpdateRangeAsync(configKeys);

		public async Task<List<Section>> GetOrderedMainSectionsAsync() => await sectionRepository.GetOrderedMainSectionsAsync();

		public Section GetFullSectionById(int id) => sectionRepository.GetById(id);

		public Section GetSectionById(int id) => sectionRepository.Get(x => x.Id == id);

		public Product GetProductById(int id) => productRepository.GetById(id);

		public ProductModel GetProductModelById(int id) => productModelRepository.GetById(id);

		public async Task UpdateSectionAndSynchronizePropertiesAsync(Section section)
		{
			await sectionRepository.CreateOrUpdateAsync(section);

			var products = await productRepository.GetProducts(section.Id);
			products.ForEach(x => x.UpdateProperties(section.GetProductPropertyList()));
			await productRepository.UpdateRangeAsync(products);

			var productModels = productModelRepository.GetProductModels(section.Id).ToList();
			productModels.ForEach(x => x.UpdateProperties(section.GetProductModelPropertyList()));
			await productModelRepository.UpdateRangeAsync(productModels);
		}

		public async Task<CommonObject> GetObjectByEnglishNameAsync(string englishName)
		{
			var section = await sectionRepository.GetItemByEnglishNameAsync(englishName);
			if (section != null)
			{
				return section;
			}

			var product = await productRepository.GetItemByEnglishNameAsync(englishName);
			if (product != null)
			{
				return product;
			}

			var productModel = await productModelRepository.GetItemByEnglishNameAsync(englishName);
			return productModel;
		}

		public async Task<SearchViewModel> CreateSearchViewModelAsync(string searchString)
		{
			return new SearchViewModel(searchString,
				await sectionRepository.GetSearchedItemsAsync(searchString),
				await productRepository.GetSearchedItemsAsync(searchString),
				await productModelRepository.GetSearchedItemsAsync(searchString));
		}
	}
}
