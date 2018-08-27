﻿using Microsoft.AspNetCore.Mvc;
using OmEnergo.Models;

namespace OmEnergo.Controllers
{
    public class AdminController : Controller
    {
        private Repository Repository { get; set; }

        public AdminController(OmEnergoContext db) => Repository = new Repository(db);

        public IActionResult Sections() => View(Repository.GetMainSections());

        public IActionResult Products(string name) => View(Repository.GetProducts(name));

        public IActionResult ProductModels(string sectionName, string productName) => View(Repository.GetProductModels(sectionName, productName));
    }
}
