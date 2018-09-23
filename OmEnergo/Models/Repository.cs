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

        public IEnumerable<Section> GetMainSections() =>
            Db.Sections.Include(x => x.ParentSection).Include(x => x.ChildSections).ToList().Where(x => x.IsMainSection());

        public IEnumerable<CommonProduct> GetProducts(string sectionName) =>
            Db.CommonProducts.Include(x => x.Section).Where(x => x.Section.Name == sectionName.Replace("_", " "));

		public IEnumerable<CommonProduct> GetSearchedProducts(string searchString) =>
			Db.CommonProducts.Where(x => x.Name.Replace(" ", "_").Contains(searchString));

        public CommonProduct GetProduct(string sectionName, string productName) =>
            Db.CommonProducts.Include(x => x.Section).Include(x => x.Models)
                .First(x => x.Section.Name.Replace(" ", "_") == sectionName && x.Name.Replace(" ", "_") == productName);

        public IEnumerable<CommonProductModel> GetProductModels(string sectionName, string productName) =>
            Db.CommonProductModels.Include(x => x.CommonProduct).Where(x => x.CommonProduct.Section.Name == sectionName && x.CommonProduct.Name == productName);

		public IEnumerable<CommonProductModel> GetSearchedProductModels(string searchString) => Db.CommonProductModels.Where(x => x.Name.Contains(searchString));

		public Section GetSection(int id) => Db.Sections.FirstOrDefault(x => x.Id == id);

        public Section GetSection(string name) => Db.Sections.FirstOrDefault(x => x.Name == name);

		public IEnumerable<Section> GetSearchedSections(string searchString) => Db.Sections.Where(x => x.Name.Contains(searchString));

		public void SaveSection(Section section)
        {
            Db.Sections.Update(section);
            Db.SaveChanges();
        }

        public CommonProduct GetCommonProduct(int id) => Db.CommonProducts.FirstOrDefault(x => x.Id == id);

        public CommonProduct GetCommonProduct(string sectionName, string productName) => 
            Db.CommonProducts.Include(x => x.Section).ToList().FirstOrDefault(x => x.Section.Name == sectionName && x.Name == productName);

        public void SaveCommonProduct(CommonProduct commonProduct)
        {
            Db.CommonProducts.Update(commonProduct);
            Db.SaveChanges();
        }

        public CommonProductModel GetCommonProductModel(int id) => Db.CommonProductModels.FirstOrDefault(x => x.Id == id);

        public void SaveCommonProductModel(CommonProductModel commonProductModel)
        {
            Db.CommonProductModels.Update(commonProductModel);
            Db.SaveChanges();
        }
    }
}
