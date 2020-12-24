using OmEnergo.Infrastructure;
using OmEnergo.Infrastructure.Database;
using OmEnergo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Services
{
	public class AdminService
	{
		private readonly SectionRepository sectionRepository;
		private readonly ProductRepository productRepository;
		private readonly ProductModelRepository productModelRepository;
		private readonly RepositoryService repositoryService;

		public AdminService(SectionRepository sectionRepository,
			ProductRepository productRepository,
			ProductModelRepository productModelRepository,
			RepositoryService repositoryService)
		{
			this.sectionRepository = sectionRepository;
			this.productRepository = productRepository;
			this.productModelRepository = productModelRepository;
			this.repositoryService = repositoryService;
		}

		public async Task CreateSectionAsync(Section section, int? parentSectionId)
		{
			if (parentSectionId != null)
			{
				section.ParentSection = sectionRepository.GetById<Section>(parentSectionId.Value);
				section.SequenceNumber = section.ParentSection.GetOrderedNestedObjects().Count() + 1;
			}
			else
			{
				section.SequenceNumber = (await sectionRepository.GetOrderedMainSectionsAsync()).Count + 1;
			}

			section.SetEnglishNameIfItsEmpty();
			await sectionRepository.CreateOrUpdateAsync(section);
		}

		public async Task EditSectionAsync(Section section, int? parentSectionId)
		{
			if (parentSectionId != null)
			{
				section.ParentSection = sectionRepository.GetById<Section>(parentSectionId.Value);
			}

			section.SetEnglishNameIfItsEmpty();
			await repositoryService.UpdateSectionAndSynchronizePropertiesAsync(section);
		}

		public async Task<string> DeleteSectionAsync(int id)
		{
			var section = sectionRepository.GetById<Section>(id);
			await sectionRepository.DeleteAsync<Section>(id);
			return section.Name;
		}

		public async Task CreateProductAsync(Product product, int? sectionId, params string[] propertyValues)
		{
			product.Section = sectionRepository.GetById<Section>(sectionId.Value);
			product.SequenceNumber = product.Section.GetOrderedNestedObjects().Count() + 1;
			product.UpdatePropertyValues(propertyValues);
			product.SetEnglishNameIfItsEmpty();
			await productRepository.CreateOrUpdateAsync(product);
		}

		public async Task EditProductAsync(Product product, int? sectionId, params string[] propertyValues)
		{
			product.Section = sectionRepository.Get<Section>(x => x.Id == sectionId);
			product.UpdatePropertyValues(propertyValues);
			product.SetEnglishNameIfItsEmpty();
			await productRepository.CreateOrUpdateAsync(product);
		}

		public async Task DeleteProductAsync(int id)
		{
			await productRepository.DeleteAsync<Product>(id);
		}

		public async Task CreateProductModelAsync(ProductModel productModel, int? sectionId, int? productId, params string[] propertyValues)
		{
			productModel.Section = sectionRepository.GetById<Section>(sectionId.GetValueOrDefault());
			productModel.Product = productRepository.GetById<Product>(productId.GetValueOrDefault());
			productModel.SequenceNumber =
				(sectionId == null ? productModel.Product.Models : productModel.Section.GetOrderedNestedObjects()).Count() + 1;
			productModel.UpdatePropertyValues(propertyValues);
			productModel.SetEnglishNameIfItsEmpty();
			await productModelRepository.CreateOrUpdateAsync(productModel);
		}

		public async Task EditProductModelAsync(ProductModel productModel, int? sectionId, int? productId, params string[] propertyValues)
		{
			productModel.Section = sectionRepository.Get<Section>(x => x.Id == sectionId);
			productModel.Product = productRepository.Get<Product>(x => x.Id == productId);
			productModel.UpdatePropertyValues(propertyValues);
			productModel.SetEnglishNameIfItsEmpty();
			await productModelRepository.CreateOrUpdateAsync(productModel);
		}

		public async Task DeleteProductModelAsync(int id)
		{
			await productModelRepository.DeleteAsync<ProductModel>(id);
		}

		public void CreateThumbnails(int maxSize)
		{
			var commonObjects = new List<CommonObject>();
			commonObjects.AddRange(sectionRepository.GetAll<Section>() ?? new List<Section>());
			commonObjects.AddRange(productRepository.GetAll<Product>() ?? new List<Product>());
			commonObjects.AddRange(productModelRepository.GetAll<ProductModel>() ?? new List<ProductModel>());
			var imageThumbnailCreator = new ImageThumbnailCreator(maxSize);
			imageThumbnailCreator.Create(commonObjects);
		}
	}
}
