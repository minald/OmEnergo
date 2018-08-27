using Microsoft.AspNetCore.Mvc;
using OmEnergo.Models;
using System;
using System.Linq;

namespace OmEnergo.Controllers
{
    public class CatalogController : Controller
    {
        private Repository Repository { get; set; }

        public CatalogController(OmEnergoContext db) => Repository = new Repository(db);

        public IActionResult Index() => View(Repository.GetSections().Where(x => x.IsMainSection()));

        public IActionResult IndustrialSinglephaseStabilizers(string series) => Products("IndustrialSinglephaseStabilizers", series);

        public IActionResult IndustrialThreephaseStabilizers(string series) => Products("IndustrialThreephaseStabilizers", series);

        public IActionResult HouseholdSinglephaseStabilizers(string series) => Products("HouseholdSinglephaseStabilizers", series);

        public IActionResult HouseholdThreephaseStabilizers(string series) => Products("HouseholdThreephaseStabilizers", series);

        public IActionResult Inverters(string series) => Products("Inverters", series);

        public IActionResult Autotransformers(string series) => Products("Autotransformers", series);

        public IActionResult Switches(string series) => Products("Switches", series);

        private IActionResult Products(string type, string series)
        {
            if (String.IsNullOrEmpty(series))
            {
                var stabilizers = Repository.GetProducts(type);
                ViewData["Title"] = type;
                return View("Stabilizers", stabilizers);
            }
            else
            {
                var stabilizer = Repository.GetProduct(series);
                ViewData["Title"] = series;
                return View("Stabilizer", stabilizer);
            }
        }
    }
}