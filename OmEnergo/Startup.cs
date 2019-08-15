using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OmEnergo.Infrastructure;
using OmEnergo.Infrastructure.Database;

namespace OmEnergo
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration) => Configuration = configuration;

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMiniProfiler().AddEntityFramework();
			string connectionString = Configuration.GetConnectionString("OmEnergoConnection");
			services.AddDbContext<OmEnergoContext>(options => options.UseSqlServer(connectionString));
			services.AddScoped<Repository>();
			services.AddScoped<FileManager>();
			services.AddScoped<ExcelReportBuilder>();
			services.AddScoped<EmailSender>();
			services.AddSession();
			services.AddMvc();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseMiddleware<DefaultImageMiddleware>();
			if (env.IsDevelopment())
			{
				app.UseMiniProfiler();
				app.UseDeveloperExceptionPage();
				app.UseBrowserLink();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseSession();
			app.UseStaticFiles();
			app.UseMvc(routes =>
			{
				routes.MapRoute("search", "poisk/{searchString?}", new { controller = "Home", action = "Search"});
				routes.MapRoute("catalog", "catalog/{name}", new { controller = "Catalog", action = "Products" });
				routes.MapRoute("home", "Home/{action=About}", new { controller = "Home" });
				routes.MapRoute("default", "{controller=Catalog}/{action=Index}");
			});
		}
	}
}
