using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace OmEnergo.Infrastructure
{
	public class DefaultImageMiddleware
	{
		private const string defaultImagePath = @"wwwroot\images\DefaultImage.jpg";

		private readonly RequestDelegate nextDelegate;

		public DefaultImageMiddleware(RequestDelegate next) => nextDelegate = next;

		public async Task Invoke(HttpContext context)
		{
			await nextDelegate(context);
			if (context.Response.StatusCode == (int)HttpStatusCode.NotFound && 
				context.Request.Headers["accept"].ToString().ToLower().StartsWith("image"))
			{
				var path = Path.Combine(Directory.GetCurrentDirectory(), defaultImagePath);
				var bytes = await File.ReadAllBytesAsync(path);
				await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
			}
		}
	}
}
