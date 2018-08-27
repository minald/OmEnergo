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

        public List<Section> GetSections() => Db.Sections.Include(x => x.ParentSection).Include(x => x.ChildrenSections).ToList();

        public IEnumerable<CommonProduct> GetProducts(string type) =>
            Db.CommonProducts.Include(x => x.Section).Where(x => x.Section.EnglishName == type).ToList();

        public CommonProduct GetProduct(string type, string name) =>
            Db.CommonProducts.Include(x => x.Section).Include(x => x.Models)
                .First(x => x.Section.EnglishName == type && x.Name.Replace(" ", "_") == name);
    }
}
