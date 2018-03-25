using Microsoft.AspNetCore.Mvc;
using OmEnergo.Models;
using System.Linq;

namespace OmEnergo.Controllers
{
    public class CatalogController : Controller
    {
        private OmEnergoContext Db { get; set; }

        private Repository Repository { get; set; }

        public CatalogController(OmEnergoContext context)
        {
            Db = context;
            Repository = new Repository(context);
        }

        public IActionResult Index()
        {
            var products = Db.Products.ToList();
            return View(products);
        }

        public IActionResult IndustrialSinglephaseStabilizers(string series)
        {
            if (series == null)
            {
                var stabilizers = Repository.GetIndustrialSinglephaseStabilizers();
                return View(stabilizers);
            }
            else
            {
                var stabilizer = Repository.GetIndustrialSinglephaseStabilizerBySeries(series);
                return View("Stabilizer", stabilizer);
            }
        }

        public IActionResult IndustrialThreephaseStabilizers(string series)
        {
            if (series == null)
            {
                var stabilizers = Repository.GetIndustrialThreephaseStabilizers();
                return View(stabilizers);
            }
            else
            {
                var stabilizer = Repository.GetIndustrialThreephaseStabilizerBySeries(series);
                return View("Stabilizer", stabilizer);
            }
        }

        public IActionResult HouseholdSinglephaseStabilizers(string series)
        {
            if(series == null)
            {
                var stabilizers = Repository.GetHouseholdSinglephaseStabilizers();
                return View(stabilizers);
            }
            else
            {
                var stabilizer = Repository.GetHouseholdSinglephaseStabilizerBySeries(series);
                return View("Stabilizer", stabilizer);
            }
        }

        public IActionResult HouseholdThreephaseStabilizers(string series)
        {
            if (series == null)
            {
                var stabilizers = Repository.GetHouseholdThreephaseStabilizers();
                return View(stabilizers);
            }
            else
            {
                var stabilizer = Repository.GetHouseholdThreephaseStabilizerBySeries(series);
                return View("Stabilizer", stabilizer);
            }
        }
    }
}