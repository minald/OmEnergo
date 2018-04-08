using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace OmEnergo.Models
{
    public class Repository
    {
        private OmEnergoContext Db { get; set; }

        public Repository()
        {
            Db = new OmEnergoContext();
        }

        public Repository(OmEnergoContext context)
        {
            Db = context;
        }

        public List<Stabilizer> GetIndustrialSinglephaseStabilizers()
        {
            return Db.Stabilizers.Where(x => x.Product.EnglishName == "IndustrialSinglephaseStabilizers").Include(x => x.Product).ToList();
        }

        public Stabilizer GetIndustrialSinglephaseStabilizerBySeries(string series)
        {
            return Db.Stabilizers.Include(x => x.Product).Include(x => x.Models)
                .First(x => x.Product.EnglishName == "IndustrialSinglephaseStabilizers" && x.Series.Replace(" ", "_") == series);
        }

        public List<Stabilizer> GetIndustrialThreephaseStabilizers()
        {
            return Db.Stabilizers.Where(x => x.Product.EnglishName == "IndustrialThreephaseStabilizers").Include(x => x.Product).ToList();
        }

        public Stabilizer GetIndustrialThreephaseStabilizerBySeries(string series)
        {
            return Db.Stabilizers.Include(x => x.Product).Include(x => x.Models)
                .First(x => x.Product.EnglishName == "IndustrialThreephaseStabilizers" && x.Series.Replace(" ", "_") == series);
        }

        public List<Stabilizer> GetHouseholdSinglephaseStabilizers()
        {
            return Db.Stabilizers.Where(x => x.Product.EnglishName == "HouseholdSinglephaseStabilizers").Include(x => x.Product).ToList();
        }

        public Stabilizer GetHouseholdSinglephaseStabilizerBySeries(string series)
        {
            return Db.Stabilizers.Include(x => x.Product).Include(x => x.Models)
                .First(x => x.Product.EnglishName == "HouseholdSinglephaseStabilizers" && x.Series.Replace(" ", "_") == series);
        }

        public List<Stabilizer> GetHouseholdThreephaseStabilizers()
        {
            return Db.Stabilizers.Where(x => x.Product.EnglishName == "HouseholdThreephaseStabilizers").Include(x => x.Product).ToList();
        }

        public Stabilizer GetHouseholdThreephaseStabilizerBySeries(string series)
        {
            return Db.Stabilizers.Include(x => x.Product).Include(x => x.Models)
                .First(x => x.Product.EnglishName == "HouseholdThreephaseStabilizers" && x.Series.Replace(" ", "_") == series);
        }
    }
}
