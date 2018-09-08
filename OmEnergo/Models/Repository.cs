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

        public IEnumerable<CommonProduct> GetProducts(string name) =>
            Db.CommonProducts.Include(x => x.Section).Where(x => x.Section.Name == name.Replace("_", " "));

        public CommonProduct GetProduct(string sectionName, string productName) =>
            Db.CommonProducts.Include(x => x.Section).Include(x => x.Models)
                .First(x => x.Section.Name.Replace(" ", "_") == sectionName && x.Name.Replace(" ", "_") == productName);

        public IEnumerable<CommonProductModel> GetProductModels(string sectionName, string productName) =>
            Db.CommonProductModels.Include(x => x.CommonProduct).Where(x => x.CommonProduct.Section.Name == sectionName && x.CommonProduct.Name == productName);

        public CommonProductModel GetCommonProductModel(int id) => Db.CommonProductModels.FirstOrDefault(x => x.Id == id);

        public void SaveCommonProductModel(CommonProductModel commonProductModel)
        {
            Db.CommonProductModels.Update(commonProductModel);
            Db.SaveChanges();
        }
    }
}
