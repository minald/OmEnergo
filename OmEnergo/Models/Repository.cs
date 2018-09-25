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

		public IEnumerable<CommonProduct> GetSearchedProducts(string searchString) =>
			Db.CommonProducts.Where(x => x.Name.Replace(" ", "_").Contains(searchString));

		public IEnumerable<CommonProductModel> GetSearchedProductModels(string searchString) => Db.CommonProductModels.Where(x => x.Name.Contains(searchString));

		public IEnumerable<Section> GetSearchedSections(string searchString) => Db.Sections.Where(x => x.Name.Contains(searchString));

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
            products.ForEach(x => x.UpdateProperties(section.GetProductPropertiesList()));
            UpdateRange(products);

            var productModels = GetProductModels(section.Id).ToList();
            productModels.ForEach(x => x.UpdateProperties(section.GetProductModelPropertiesList()));
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

        public Section GetSection(string name) => Db.Sections.FirstOrDefault(x => x.Name == name);

        public IEnumerable<Section> GetMainSections() =>
            Db.Sections.Include(x => x.ParentSection).Include(x => x.ChildSections).ToList().Where(x => x.IsMainSection());

        public CommonProduct GetProduct(int sectionId) =>
            Db.CommonProducts.Include(x => x.Section).Include(x => x.Models).First(x => x.Section.Id == sectionId);

        public CommonProduct GetProduct(string sectionName, string productName) =>
            Db.CommonProducts.Include(x => x.Section).Include(x => x.Models)
                .First(x => x.Section.Name == sectionName && x.Name == productName);

        public IEnumerable<CommonProduct> GetProducts(int sectionId) =>
            Db.CommonProducts.Include(x => x.Section).Where(x => x.Section.Id == sectionId);

        public IEnumerable<CommonProduct> GetProducts(string sectionName) =>
            Db.CommonProducts.Include(x => x.Section).Where(x => x.Section.Name == sectionName);

        public CommonProductModel GetProductModel(int sectionId) =>
            Db.CommonProductModels.Include(x => x.CommonProduct).First(x => x.CommonProduct.Section.Id == sectionId);

        public IEnumerable<CommonProductModel> GetProductModels(int sectionId) =>
            Db.CommonProductModels.Include(x => x.CommonProduct).Where(x => x.CommonProduct.Section.Id == sectionId);

        public IEnumerable<CommonProductModel> GetProductModels(string sectionName, string productName) =>
            Db.CommonProductModels.Include(x => x.CommonProduct)
                .Where(x => x.CommonProduct.Section.Name == sectionName && x.CommonProduct.Name == productName);
    }
}
