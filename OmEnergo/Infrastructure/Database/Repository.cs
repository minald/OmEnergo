using Microsoft.EntityFrameworkCore;
using OmEnergo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OmEnergo.Infrastructure.Database
{
	public class Repository
	{
		private OmEnergoContext db { get; set; }

		public Repository() => db = new OmEnergoContext();

		public Repository(OmEnergoContext context) => db = context;

		#region Methods which must be optimized

		public IEnumerable<Section> GetOrderedMainSections() => db.Sections.Include(x => x.ParentSection)
			.ToList().Where(x => x.IsMainSection()).OrderBy(x => x.SequenceNumber);

		public Section GetSection(string name) => db.Sections.Include(x => x.Products).Include(x => x.ProductModels)
			.Include(x => x.ChildSections).Include(x => x.ParentSection).FirstOrDefault(x => x.EnglishName == name);

		public Product GetProduct(string name) =>
			db.Products.Include(x => x.Section).Include(x => x.Models).FirstOrDefault(x => x.EnglishName == name);

		public ProductModel GetProductModel(string name) =>
			db.ProductModels.Include(x => x.Section).Include(x => x.Product).ThenInclude(x => x.Section)
				.FirstOrDefault(x => x.EnglishName == name);

		public IEnumerable<Section> GetFullCatalog() =>
			db.Sections.Include(x => x.ProductModels).Include(x => x.Products).ThenInclude(x => x.Models).ToList().Where(x => x.IsMainSection());

		public IEnumerable<Section> GetSearchedSections(string searchString) =>
			db.Sections.Where(x => x.Name.Contains(searchString));

		public IEnumerable<Product> GetSearchedProducts(string searchString) =>
			db.Products.Include(x => x.Section).Where(x => x.Name.Contains(searchString));

		public IEnumerable<ProductModel> GetSearchedProductModels(string searchString) =>
			db.ProductModels.Include(x => x.Section).Include(x => x.Product).ThenInclude(x => x.Section)
				.Where(x => x.Name.Contains(searchString));

		#endregion

		public CommonObject GetObjectByEnglishName(string englishName)
		{
			var section = GetSection(englishName);
			if (section != null)
			{
				return section;
			}

			var product = GetProduct(englishName);
			if (product != null)
			{
				return product;
			}

			var productModel = GetProductModel(englishName);
			return productModel;
		}

		public T Get<T>(Func<T, bool> predicate) where T : UniqueObject => db.Set<T>().FirstOrDefault(predicate);

		public void Update<T>(T obj) where T : UniqueObject
		{
			var existingItem = db.Set<T>().Find(obj.Id);
			if (existingItem == null)
			{
				db.Add(obj);
			}
			else
			{
				db.Entry(existingItem).CurrentValues.SetValues(obj);
			}

			db.SaveChanges();
		}

		public void UpdateSectionAndSynchronizeProperties(Section section)
		{
			Update(section);

			var products = GetProducts(section.Id).ToList();
			products.ForEach(x => x.UpdateProperties(section.GetProductPropertyList()));
			UpdateRange(products);

			var productModels = GetProductModels(section.Id).ToList();
			productModels.ForEach(x => x.UpdateProperties(section.GetProductModelPropertyList()));
			UpdateRange(productModels);
		}

		public void UpdateRange<T>(IEnumerable<T> obj) where T : UniqueObject
		{
			db.Set<T>().UpdateRange(obj);
			db.SaveChanges();
		}

		public void Delete<T>(int id) where T : UniqueObject
		{
			db.Set<T>().Remove(db.Set<T>().FirstOrDefault(x => x.Id == id));
			db.SaveChanges();
		}

		#region Sections

		public virtual IEnumerable<Section> GetAllSections() => db.Sections
			.Include(x => x.Products).ThenInclude(x => x.Models)
			.Include(x => x.ChildSections).ThenInclude(x => x.Products)
			.Include(x => x.ChildSections).ThenInclude(x => x.ProductModels)
			.Include(x => x.ProductModels)
			.Include(x => x.ParentSection);

		public IEnumerable<Section> GetSections(Func<Section, bool> predicate) => GetAllSections().Where(predicate);

		public Section GetSection(int id) => GetAllSections().FirstOrDefault(x => x.Id == id);

		#endregion

		#region Products

		public virtual IEnumerable<Product> GetAllProducts() => db.Products.Include(x => x.Section).Include(x => x.Models);

		public Product GetProduct(int id) => GetAllProducts().FirstOrDefault(x => x.Id == id);

		public IEnumerable<Product> GetProducts(int sectionId) => GetAllProducts().Where(x => x.Section.Id == sectionId);

		#endregion

		#region ProductModels

		public virtual IEnumerable<ProductModel> GetAllProductModels() => db.ProductModels
			.Include(x => x.Section)
			.Include(x => x.Product).ThenInclude(x => x.Section);

		public IEnumerable<ProductModel> GetProductModels(int sectionId) =>
			GetAllProductModels().Where(x => x.Section?.Id == sectionId || x.Product?.Section?.Id == sectionId);

		public ProductModel GetProductModel(int id) => GetAllProductModels().FirstOrDefault(x => x.Id == id);

		#endregion

		#region ConfigKeys

		public virtual List<ConfigKey> GetAllConfigKeys() => db.ConfigKeys.ToList();

		public string GetConfigValue(string key) =>
			db.ConfigKeys.FirstOrDefault(x => x.Key.ToLower() == key.ToLower())?.Value ?? "";

		#endregion
	}
}
