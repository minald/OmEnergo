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

		public IActionResult Products(string productName, string series)
		{
			if (String.IsNullOrEmpty(series))
			{
				ViewData["Title"] = productName.Replace("_", " ");
				return View("Products", Repository.GetProducts(productName));
			}
			else
			{
				ViewData["Title"] = series.Replace("_", " ");
				return View("Product", Repository.GetProduct(productName, series));
			}
		}

		public IActionResult Search(string searchString) => View(new SearchViewModel(Repository.GetSearchedSections(searchString), 
				Repository.GetSearchedProducts(searchString), Repository.GetSearchedProductModels(searchString)));
	}
}