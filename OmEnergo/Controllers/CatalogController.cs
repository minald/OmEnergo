using Microsoft.AspNetCore.Mvc;
using OmEnergo.Models;
using OmEnergo.Models.ViewModels;
using System;

namespace OmEnergo.Controllers
{
	public class CatalogController : Controller
	{
		private Repository Repository { get; set; }

		public CatalogController(Repository repository) => Repository = repository;

		public IActionResult Index() => View(Repository.GetMainSections());

		public IActionResult Search(string searchString) => View(new SearchViewModel(searchString,
            Repository.GetSearchedSections(searchString), Repository.GetSearchedProducts(searchString),
            Repository.GetSearchedProductModels(searchString)));
        
        public IActionResult Products(string sectionName, string productName, string productModelName)
        {
            if (String.IsNullOrEmpty(productName))
            {
                ViewData["Title"] = sectionName;
                return View("Section", Repository.GetSection(sectionName));
            }
            else if (String.IsNullOrEmpty(productModelName))
            {
                ViewData["Title"] = productName;
                return View("Product", Repository.GetProduct(sectionName, productName));
            }
            else
            {
                ViewData["Title"] = productModelName;
                return View("ProductModel", Repository.GetProductModel(sectionName, productName, productModelName));
            }
        }
    }
}
