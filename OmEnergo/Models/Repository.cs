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

        public List<Stabilizer> GetStabilizers(string type) =>
            Db.Stabilizers.Where(x => x.Product.EnglishName == type + "Stabilizers").Include(x => x.Product).ToList();

        public Stabilizer GetStabilizerBySeries(string type, string series) =>
            Db.Stabilizers.Include(x => x.Product).Include(x => x.Models)
                .First(x => x.Product.EnglishName == type + "Stabilizers" && x.Series.Replace(" ", "_") == series);
    }
}
