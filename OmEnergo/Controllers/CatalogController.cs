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

        public IActionResult Index() => View(Repository.GetMainSections());

        public IActionResult IndustrialSinglephaseStabilizers(string series) => Products("Промышленные однофазные стабилизаторы", series);

        public IActionResult IndustrialThreephaseStabilizers(string series) => Products("Промышленные трехфазные стабилизаторы", series);

        public IActionResult HouseholdSinglephaseStabilizers(string series) => Products("Бытовые однофазные стабилизаторы", series);

        public IActionResult HouseholdThreephaseStabilizers(string series) => Products("Бытовые трехфазные стабилизаторы", series);

        public IActionResult Inverters(string series) => Products("Источники бесперебойного питания", series);

        public IActionResult Autotransformers(string series) => Products("Лабораторные автотрансформаторы", series);

        public IActionResult Switches(string series) => Products("Выключатели и переключатели", series);

        private IActionResult Products(string productName, string series)
        {
            if (String.IsNullOrEmpty(series))
            {
                ViewData["Title"] = productName;
                return View("Products", Repository.GetProducts(productName));
            }
            else
            {
                ViewData["Title"] = series.Replace("_", " ");
                return View("Product", Repository.GetProduct(productName, series));
            }
        }
    }
}