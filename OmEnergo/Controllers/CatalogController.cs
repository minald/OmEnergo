using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmEnergo.Models;
using System.Linq;

namespace OmEnergo.Controllers
{
    public class CatalogController : Controller
    {
        private OmEnergoContext Db;

        public CatalogController(OmEnergoContext db)
        {
            Db = db;
        }

        public IActionResult Index()
        {
            var products = Db.Products.ToList();
            return View(products);
        }

        public IActionResult IndustrialSinglephaseStabilizers(string id)
        {
            if (id == null)
            {
                var stabilizers = Db.Stabilizers.Include(x => x.Product).ToList();
                return View(stabilizers);
            }
            else
            {
                var stabilizer = Db.Stabilizers.Include(x => x.Product).First(x => x.Series.Replace(" ", "_") == id);
                return View("Stabilizer", stabilizer);
            }
        }

        public IActionResult IndustrialThreephaseStabilizers(string id)
        {
            if (id == null)
            {
                var stabilizers = Db.Stabilizers.Include(x => x.Product).ToList();
                return View(stabilizers);
            }
            else
            {
                var stabilizer = Db.Stabilizers.Include(x => x.Product).First(x => x.Series.Replace(" ", "_") == id);
                return View("Stabilizer", stabilizer);
            }
        }

        public IActionResult HouseholdSinglephaseStabilizers(string id)
        {
            if(id == null)
            {
                var stabilizers = Db.Stabilizers.Include(x => x.Product).ToList();
                return View(stabilizers);
            }
            else
            {
                var stabilizer = Db.Stabilizers.Include(x => x.Product).First(x => x.Series.Replace(" ", "_") == id);
                return View("Stabilizer", stabilizer);
            }
        }

        public IActionResult HouseholdThreephaseStabilizers(string id)
        {
            if (id == null)
            {
                var stabilizers = Db.Stabilizers.Include(x => x.Product).ToList();
                return View(stabilizers);
            }
            else
            {
                var stabilizer = Db.Stabilizers.Include(x => x.Product).First(x => x.Series.Replace(" ", "_") == id);
                return View("Stabilizer", stabilizer);
            }
        }
    }
}