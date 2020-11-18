using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvcLocalizationSample.Data;
using MvcLocalizationSample.LocalizationResources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using XLocalizer;
using XLocalizer.Routing;
using XLocalizer.Translate;
using XLocalizer.Translate.MyMemoryTranslate;
using XLocalizer.Xml;

namespace MvcLocalizationSample
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddHttpClient<ITranslator, MyMemoryTranslateService>();
            services.AddSingleton<IXResourceProvider, XmlResourceProvider>();

            services.Configure<RequestLocalizationOptions>(ops =>
            {
                var cultures = new CultureInfo[] { new CultureInfo("en"), new CultureInfo("tr"), new CultureInfo("ar") };
                ops.SupportedCultures = cultures;
                ops.SupportedUICultures = cultures;
                ops.DefaultRequestCulture = new RequestCulture("en");
                ops.RequestCultureProviders.Insert(0, new RouteSegmentRequestCultureProvider(cultures));
            });

            services.AddControllersWithViews()
                // add culture route segment for controllers e.g. /en/Home/Index
                .AddMvcOptions(ops => ops.Conventions.Insert(0, new RouteTemplateModelConventionMvc()))
                
                // add culture route for Identity pages e.g. /en/Identity/Account/Login
                .AddRazorPagesOptions(ops => ops.Conventions.Insert(0, new RouteTemplateModelConventionRazorPages()))
                .AddXLocalizer<LocSource, MyMemoryTranslateService>(ops =>
                {
                    ops.AutoAddKeys = true;
                    ops.AutoTranslate = true;
                    ops.TranslateFromCulture = "en";
                    ops.UseExpressMemoryCache = true;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
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
            
            app.UseRequestLocalization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
