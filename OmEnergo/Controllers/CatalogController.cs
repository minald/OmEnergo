using Microsoft.AspNetCore.Mvc;
using OmEnergo.Models;
using System.Linq;

namespace OmEnergo.Controllers
{
    public class CatalogController : Controller
    {
        private OmEnergoContext Db { get; }

        public CatalogController(OmEnergoContext db)
        {
            Db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Stabilizers()
        {
            var stabilizers = Db.Stabilizers.ToList();
            return View(stabilizers);
        }
    }
}