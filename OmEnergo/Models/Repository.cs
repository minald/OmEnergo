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

        public IEnumerable<CommonProduct> GetProducts(string type) =>
            Db.CommonProducts.Include(x => x.Section).Where(x => x.Section.EnglishName == type);

        public CommonProduct GetProduct(string sectionName, string name) =>
            Db.CommonProducts.Include(x => x.Section).Include(x => x.Models)
                .First(x => x.Section.EnglishName == sectionName && x.Name.Replace(" ", "_") == name);
    }
}
