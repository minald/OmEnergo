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

        public T Get<T>(int id) where T : CommonObject => Db.Set<T>().FirstOrDefault(x => x.Id == id);

        public void Update<T>(T obj) where T : CommonObject
        {
            Db.Set<T>().Update(obj);
            Db.SaveChanges();
        }

        public void Delete<T>(int id) where T : CommonObject
        {
            Db.Set<T>().Remove(Db.Set<T>().FirstOrDefault(x => x.Id == id));
            Db.SaveChanges();
        }

        public IEnumerable<Section> GetMainSections() =>
            Db.Sections.Include(x => x.ParentSection).Include(x => x.ChildSections).ToList().Where(x => x.IsMainSection());

        public IEnumerable<CommonProduct> GetProducts(string sectionName) =>
            Db.CommonProducts.Include(x => x.Section).Where(x => x.Section.Name == sectionName.Replace("_", " "));

        public CommonProduct GetProduct(string sectionName, string productName) =>
            Db.CommonProducts.Include(x => x.Section).Include(x => x.Models)
                .First(x => x.Section.Name.Replace(" ", "_") == sectionName && x.Name.Replace(" ", "_") == productName);

        public IEnumerable<CommonProductModel> GetProductModels(string sectionName, string productName) =>
            Db.CommonProductModels.Include(x => x.CommonProduct).Where(x => x.CommonProduct.Section.Name == sectionName && x.CommonProduct.Name == productName);

        public Section GetSection(string name) => Db.Sections.FirstOrDefault(x => x.Name == name);

        public CommonProduct GetCommonProduct(string sectionName, string productName) => 
            Db.CommonProducts.Include(x => x.Section).ToList().FirstOrDefault(x => x.Section.Name == sectionName && x.Name == productName);
    }
}
