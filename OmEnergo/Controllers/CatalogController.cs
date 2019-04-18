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
                var section = Repository.GetSection(sectionName);
                ViewData["Title"] = section.Name;
                return View("Section", section);
            }
            else if (String.IsNullOrEmpty(productModelName))
            {
                var product = Repository.GetProduct(sectionName, productName);
                ViewData["Title"] = product.Name;
                return View("Product", product);
            }
            else
            {
                var productModel = Repository.GetProductModel(sectionName, productName, productModelName);
                ViewData["Title"] = productModel.Name;
                return View("ProductModel", productModel);
            }
        }
    }
}
