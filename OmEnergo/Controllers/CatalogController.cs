using Microsoft.AspNetCore.Mvc;
using OmEnergo.Models;
using OmEnergo.Models.ViewModels;
using System;

namespace OmEnergo.Controllers
{
	public class CatalogController : Controller
	{
		private Repository Repository { get; set; }

		public CatalogController(OmEnergoContext db) => Repository = new Repository(db);

		public IActionResult Index() => View(Repository.GetMainSections());

		public IActionResult Search(string searchString) => View(new SearchViewModel(Repository.GetSearchedSections(searchString), 
				Repository.GetSearchedProducts(searchString), Repository.GetSearchedProductModels(searchString)));
        
        public IActionResult Products(string sectionName, string productName)
        {
            sectionName = sectionName.Replace("_", " ");
            if (String.IsNullOrEmpty(productName))
            {
                ViewData["Title"] = sectionName;
                return View("Section", Repository.GetSection(sectionName));
            }
            else
            {
                productName = productName.Replace("_", " ");
                ViewData["Title"] = productName;
                return View("Product", Repository.GetProduct(sectionName, productName));
            }
        }
    }
}