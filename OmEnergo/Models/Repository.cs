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
            Db.Stabilizers.Where(x => x.Section.EnglishName == type + "Stabilizers").Include(x => x.Section).ToList();

        public Stabilizer GetStabilizerBySeries(string type, string series) =>
            Db.Stabilizers.Include(x => x.Section).Include(x => x.Models)
                .First(x => x.Section.EnglishName == type + "Stabilizers" && x.Series.Replace(" ", "_") == series);

        public List<Inverter> GetInverters() => Db.Inverters.Include(x => x.Section).ToList();

        public Inverter GetInverterBySeries(string series) => 
            Db.Inverters.Include(x => x.Section).Include(x => x.Models).First(x => x.Series.Replace(" ", "_") == series);

        public List<Autotransformer> GetAutotransformers() => Db.Autotransformers.Include(x => x.Section).ToList();

        public Autotransformer GetAutotransformerBySeries(string series) =>
            Db.Autotransformers.Include(x => x.Section).Include(x => x.Models).First(x => x.Series.Replace(" ", "_") == series);

        public List<Switch> GetSwitches() => Db.Switches.Include(x => x.Section).ToList();

        public Switch GetSwitchBySeries(string series) =>
            Db.Switches.Include(x => x.Section).Include(x => x.Models).First(x => x.Series.Replace(" ", "_") == series);
    }
}
