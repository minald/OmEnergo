using System.IO;
using Microsoft.AspNetCore.Mvc;
using OmEnergo.Infrastructure;

namespace OmEnergo.Controllers
{
	public class DocumentController : Controller
	{
		public IActionResult Look(string path)
		{
			var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			return new FileStreamResult(fileStream, FileManager.GetContentType(path));
		}

		public IActionResult Download(string path)
		{
			var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			return File(fileStream, FileManager.GetContentType(path), FileManager.GetFileName(path));
		}
	}
}
