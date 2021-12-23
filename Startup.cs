using EmailService;
using Gmt_Asset_Tracker.Data;
using Gmt_Asset_Tracker.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Gmt_Asset_Tracker.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmt_Asset_Tracker
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<AssetTrackerContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
			services.AddDbContext<ApplicationContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("sqlConnection")));

			services.AddDatabaseDeveloperPageExceptionFilter();
			services.AddControllersWithViews();

			services.AddIdentity<User, IdentityRole>(opt =>
			{
				opt.Password.RequiredLength = 7;
				opt.Password.RequireDigit = false;
				opt.Password.RequireUppercase = false;

				opt.User.RequireUniqueEmail = true;
			})
			.AddEntityFrameworkStores<ApplicationContext>()
			.AddDefaultTokenProviders();

			services.Configure<DataProtectionTokenProviderOptions>(opt =>
			   opt.TokenLifespan = TimeSpan.FromHours(2));

			services.AddAutoMapper(typeof(Startup));
			services.AddAuthorization(options =>
		   options.AddPolicy("Administrator",
			   policy => policy.RequireClaim("User")));

			var emailConfig = Configuration
				.GetSection("EmailConfiguration")
				.Get<EmailConfiguration>();
			services.AddSingleton(emailConfig);
			services.AddScoped<IEmailSender, EmailSender>();

			services.AddControllersWithViews();

			services.AddDatabaseDeveloperPageExceptionFilter();
			services.AddControllersWithViews();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Asset}/{action=Index}/{id?}");
			});
		}
	}
}