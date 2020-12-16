using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OmEnergo.Infrastructure;
using OmEnergo.Infrastructure.Database;
using OmEnergo.Infrastructure.Excel;
using OmEnergo.Resources;

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
			services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<OmEnergoContext>();
			services.AddScoped<Repository>();
			services.AddScoped<SectionRepository>();
			services.AddScoped<ProductRepository>();
			services.AddScoped<ProductModelRepository>();
			services.AddScoped<ConfigKeyRepository>();
			services.AddScoped<CompoundRepository>();

			services.AddLocalization();
			services.AddSingleton<IStringLocalizer, StringLocalizer<SharedResource>>();

			services.AddSingleton(sp => new FileManager(
				sp.GetRequiredService<IHostingEnvironment>().WebRootPath,
				sp.GetRequiredService<IStringLocalizer>()));
			services.AddScoped<AdminFileManager>();
			services.AddScoped<ExcelWriter>();
			services.AddScoped<ExcelDbUpdater>();
			services.AddScoped<EmailSender>();

			services.AddSession();
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddFile(Configuration.GetSection("Logging"));

			app.UseMiddleware<DefaultImageMiddleware>();
			if (env.IsDevelopment())
			{
				app.UseMiniProfiler();
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseSession();
			app.UseStaticFiles();

			app.UseAuthentication();

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
