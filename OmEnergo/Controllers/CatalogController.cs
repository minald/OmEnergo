using Microsoft.AspNetCore.Mvc;
using OmEnergo.Models;
using System.Linq;

namespace OmEnergo.Controllers
{
    public class CatalogController : Controller
    {
        private OmEnergoContext Db;

        public CatalogController(OmEnergoContext db)
        {
            Db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Stabilizers(string id)
        {
            if(id == null)
            {
                var stabilizers = Db.Stabilizers.ToList();
                return View(stabilizers);
            }
            else
            {
                var stabilizer = Db.Stabilizers.First(x => x.Series.Replace(" ", "_") == id);
                return View("Stabilizer", stabilizer);
            }
        }
    }
}