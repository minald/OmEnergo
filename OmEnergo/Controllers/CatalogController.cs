﻿using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            var products = Db.Products.ToList();
            return View(products);
        }

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
    }
}