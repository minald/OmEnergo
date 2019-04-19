using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OmEnergo.Models
{
    public class Repository
    {
        private OmEnergoContext Db { get; set; }

        public Repository() => Db = new OmEnergoContext();

        public Repository(OmEnergoContext context) => Db = context;

        #region Methods which must be optimized

        public IEnumerable<Section> GetMainSections() => Db.Sections.Include(x => x.ParentSection).Include(x => x.ChildSections)
            .Include(x => x.Products).Include(x => x.ProductModels).ToList().Where(x => x.IsMainSection());

        public Section GetSection(string name) => Db.Sections.Include(x => x.Products).Include(x => x.ProductModels)
            .Include(x => x.ChildSections).Include(x => x.ParentSection).FirstOrDefault(x => x.EnglishName == name);

        public Product GetProduct(string name) =>
            Db.Products.Include(x => x.Section).Include(x => x.Models).FirstOrDefault(x => x.EnglishName == name);

        public ProductModel GetProductModel(string name) =>
            Db.ProductModels.Include(x => x.Section).Include(x => x.Product).ThenInclude(x => x.Section)
                .FirstOrDefault(x => x.EnglishName == name);

        public IEnumerable<Section> GetFullCatalog() =>
            Db.Sections.Include(x => x.ProductModels).Include(x => x.Products).ThenInclude(x => x.Models).ToList().Where(x => x.IsMainSection());

        public IEnumerable<Section> GetSearchedSections(string searchString) =>
            Db.Sections.Where(x => x.Name.Contains(searchString));

        public IEnumerable<Product> GetSearchedProducts(string searchString) =>
            Db.Products.Include(x => x.Section).Where(x => x.Name.Contains(searchString));

        public IEnumerable<ProductModel> GetSearchedProductModels(string searchString) =>
            Db.ProductModels.Include(x => x.Section).Include(x => x.Product).ThenInclude(x => x.Section)
                .Where(x => x.Name.Contains(searchString));

        #endregion

        public T Get<T>(Func<T, bool> predicate) where T : CommonObject => Db.Set<T>().FirstOrDefault(predicate);

        public void Update<T>(T obj) where T : CommonObject
        {
            var existingItem = Db.Set<T>().Find(obj.Id);
            if (existingItem == null)
            {
                Db.Add(obj);
            }
            else
            {
                Db.Entry(existingItem).CurrentValues.SetValues(obj);
            }

            Db.SaveChanges();
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

        public void UpdateRange<T>(IEnumerable<T> obj) where T : CommonObject
        {
            Db.Set<T>().UpdateRange(obj);
            Db.SaveChanges();
        }

        public void Delete<T>(int id) where T : CommonObject
        {
            Db.Set<T>().Remove(Db.Set<T>().FirstOrDefault(x => x.Id == id));
            Db.SaveChanges();
        }

        #region Sections

        public IEnumerable<Section> GetAllSections() => Db.Sections
            .Include(x => x.Products).ThenInclude(x => x.Models)
            .Include(x => x.ChildSections).ThenInclude(x => x.Products)
            .Include(x => x.ChildSections).ThenInclude(x => x.ProductModels)
            .Include(x => x.ProductModels)
            .Include(x => x.ParentSection);

        public IEnumerable<Section> GetSections(Func<Section, bool> predicate) => GetAllSections().Where(predicate);

        public Section GetSection(int id) => GetAllSections().FirstOrDefault(x => x.Id == id);

        #endregion

        #region Products

        public IEnumerable<Product> GetAllProducts() => Db.Products.Include(x => x.Section).Include(x => x.Models);

        public Product GetProduct(int id) => GetAllProducts().FirstOrDefault(x => x.Id == id);

        public IEnumerable<Product> GetProducts(int sectionId) => GetAllProducts().Where(x => x.Section.Id == sectionId);

        #endregion

        #region ProductModels

        public IEnumerable<ProductModel> GetAllProductModels() => Db.ProductModels
            .Include(x => x.Section)
            .Include(x => x.Product).ThenInclude(x => x.Section);

        public IEnumerable<ProductModel> GetProductModels(int sectionId) =>
            GetAllProductModels().Where(x => x.Section?.Id == sectionId || x.Product?.Section?.Id == sectionId);

        public ProductModel GetProductModel(int id) => GetAllProductModels().FirstOrDefault(x => x.Id == id);

        #endregion

        #region ConfigKeys

        public IEnumerable<ConfigKey> GetAllConfigKeys() => Db.ConfigKeys.ToList();

        public string GetConfigValue(string key) =>
            Db.ConfigKeys.FirstOrDefault(x => x.Key.ToLower() == key.ToLower())?.Value ?? "";

        #endregion
    }
}
