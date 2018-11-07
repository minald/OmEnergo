using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace OmEnergo.Models
{
    public class Repository
    {
        private OmEnergoContext Db { get; set; }

        public Repository() => Db = new OmEnergoContext();

        public Repository(OmEnergoContext context) => Db = context;

		public IEnumerable<Section> GetFullCatalog() =>
			Db.Sections.Include(x => x.ProductModels).Include(x => x.Products).ThenInclude(x => x.Models).ToList().Where(x => x.IsMainSection());

		public IEnumerable<Product> GetSearchedProducts(string searchString) =>
			Db.Products.Where(x => x.Name.Replace(" ", "_").Contains(searchString));

		public IEnumerable<ProductModel> GetSearchedProductModels(string searchString) => 
            Db.ProductModels.Where(x => x.Name.Contains(searchString));

		public IEnumerable<Section> GetSearchedSections(string searchString) => 
            Db.Sections.Where(x => x.Name.Contains(searchString));

        public T Get<T>(int? id) where T : CommonObject => Db.Set<T>().FirstOrDefault(x => x.Id == id);

        public void Update<T>(T obj) where T : CommonObject
        {
            Db.Set<T>().Update(obj);
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

        public void UpdateSectionsSequenceNumbers(int? parentSectionId, int deletedSequenceNumber)
        {
            var updatedSections = Db.Sections.OrderBy(x => x.SequenceNumber)
                .Where(x => x.SequenceNumber > deletedSequenceNumber && x.ParentSection.Id == parentSectionId);
            updatedSections.ToList().ForEach(x => x.SequenceNumber--);
            Db.Sections.UpdateRange(updatedSections);
            Db.SaveChanges();
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

        public Section GetSection(int id) => Db.Sections.Include(x => x.Products).Include(x => x.ProductModels)
            .Include(x => x.ChildSections).Include(x => x.ParentSection).FirstOrDefault(x => x.Id == id);

        public Section GetSection(string name) => Db.Sections.Include(x => x.Products).Include(x => x.ProductModels)
            .Include(x => x.ChildSections).Include(x => x.ParentSection).FirstOrDefault(x => x.Name == name);

        public IEnumerable<Section> GetMainSections() =>
            Db.Sections.Include(x => x.ParentSection).Include(x => x.ChildSections).ToList().Where(x => x.IsMainSection());

        public Product GetProduct(int id) => Db.Products.Include(x => x.Section)
            .Include(x => x.Models).FirstOrDefault(x => x.Id == id);

        public Product GetProduct(string sectionName, string productName) =>
            Db.Products.Include(x => x.Section).Include(x => x.Models)
                .First(x => x.Section.Name == sectionName && x.Name == productName);

        public IEnumerable<Product> GetProducts(int sectionId) =>
            Db.Products.Include(x => x.Section).Where(x => x.Section.Id == sectionId);

        public IEnumerable<Product> GetProducts(string sectionName) =>
            Db.Products.Include(x => x.Section).Where(x => x.Section.Name == sectionName);

		public ProductModel GetProductModel(int id) =>
			Db.ProductModels.Include(x => x.Section).Include(x => x.Product).FirstOrDefault(x => x.Id == id);

		public IEnumerable<ProductModel> GetProductModels(int sectionId) =>
            Db.ProductModels.Include(x => x.Product).Where(x => x.Section.Id == sectionId || x.Product.Section.Id == sectionId);

        public IEnumerable<ProductModel> GetProductModels(string sectionName, string productName) =>
            Db.ProductModels.Include(x => x.Product)
                .Where(x => x.Product.Section.Name == sectionName && x.Product.Name == productName);
    }
}
