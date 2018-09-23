using Microsoft.AspNetCore.Mvc;
using OmEnergo.Models;
using System;

namespace OmEnergo.Controllers
{
    public class CatalogController : Controller
    {
        private Repository Repository { get; set; }

        public CatalogController(OmEnergoContext db) => Repository = new Repository(db);

        public IActionResult Index() => View(Repository.GetMainSections());

        public IActionResult Products(string sectionName, string productName)
        {
            sectionName = sectionName.Replace("_", " ");
            productName = productName.Replace("_", " ");
            if (String.IsNullOrEmpty(productName))
            {
                ViewData["Title"] = sectionName;
                return View("Products", Repository.GetProducts(sectionName));
            }
            else
            {
                ViewData["Title"] = productName;
                return View("Product", Repository.GetProduct(sectionName, productName));
            }
        }
    }
}