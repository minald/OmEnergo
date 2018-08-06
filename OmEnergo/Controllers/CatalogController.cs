using Microsoft.AspNetCore.Mvc;
using OmEnergo.Models;
using System;
using System.Linq;

namespace OmEnergo.Controllers
{
    public class CatalogController : Controller
    {
        private OmEnergoContext Db { get; set; }

        private Repository Repository { get; set; }

        public CatalogController(OmEnergoContext db)
        {
            Db = db;
            Repository = new Repository(db);
        }

        public IActionResult Index() => View(Db.Sections.ToList());

        public IActionResult IndustrialSinglephaseStabilizers(string series) => Stabilizers("IndustrialSinglephase", series);

        public IActionResult IndustrialThreephaseStabilizers(string series) => Stabilizers("IndustrialThreephase", series);

        public IActionResult HouseholdSinglephaseStabilizers(string series) => Stabilizers("HouseholdSinglephase", series);

        public IActionResult HouseholdThreephaseStabilizers(string series) => Stabilizers("HouseholdThreephase", series);

        private IActionResult Stabilizers(string type, string series)
        {
            if (String.IsNullOrEmpty(series))
            {
                var stabilizers = Repository.GetStabilizers(type);
                return View("Stabilizers", stabilizers);
            }
            else
            {
                var stabilizer = Repository.GetStabilizerBySeries(type, series);
                return View("Stabilizer", stabilizer);
            }
        }

        public IActionResult Inverters(string series)
        {
            if (String.IsNullOrEmpty(series))
            {
                var inverters = Repository.GetInverters();
                return View("Inverters", inverters);
            }
            else
            {
                var inverter = Repository.GetInverterBySeries(series);
                return View("Inverter", inverter);
            }
        }

        public IActionResult Autotransformers(string series)
        {
            if (String.IsNullOrEmpty(series))
            {
                var autotransformers = Repository.GetAutotransformers();
                return View("Autotransformers", autotransformers);
            }
            else
            {
                var autotransformer = Repository.GetAutotransformerBySeries(series);
                return View("Autotransformer", autotransformer);
            }
        }

        public IActionResult Switches(string series)
        {
            if (String.IsNullOrEmpty(series))
            {
                var switches = Repository.GetSwitches();
                return View("Switches", switches);
            }
            else
            {
                var switch_ = Repository.GetSwitchBySeries(series);
                return View("Switch", switch_);
            }
        }
    }
}