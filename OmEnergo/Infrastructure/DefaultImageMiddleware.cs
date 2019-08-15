using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace OmEnergo.Infrastructure
{
	public class DefaultImageMiddleware
	{ 
		private readonly RequestDelegate nextDelegate;

		public DefaultImageMiddleware(RequestDelegate next) => nextDelegate = next;

		public async Task Invoke(HttpContext context)
		{
			await nextDelegate(context);
			if (context.Response.StatusCode == 404 && 
				context.Request.Headers["accept"].ToString().ToLower().StartsWith("image"))
			{
				string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\DefaultImage.jpg");
				byte[] bytes = await File.ReadAllBytesAsync(path);
				await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
			}
		}
	}
}
