using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using XmlLocalizationSample.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XLocalizer.Translate.MyMemoryTranslate;
using XLocalizer.Translate;
using XLocalizer;
using XLocalizer.Xml;
using System.Globalization;
using XLocalizer.Routing;
using XmlLocalizationSample.LocalizationResources;
using XLocalizer.Translate.GoogleTranslate;
using XLocalizer.Translate.IBMWatsonTranslate;
using XLocalizer.Translate.SystranTranslate;
using XLocalizer.Translate.YandexTranslate;

namespace XmlLocalizationSample
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
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<RequestLocalizationOptions>(ops =>
            {
                var cultures = new CultureInfo[] { new CultureInfo("en"), new CultureInfo("tr"), new CultureInfo("ar") };
                ops.SupportedCultures = cultures;
                ops.SupportedUICultures = cultures;
                ops.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");

                // Optional: add custom provider to support localization based on route value {culture}
                ops.RequestCultureProviders.Insert(0, new RouteSegmentRequestCultureProvider(cultures));
            });
            
            // Optional: To enable online translation register one or more translation services
            services.AddSingleton<ITranslator, IBMWatsonTranslateService>();
            services.AddHttpClient<ITranslator, YandexTranslateService>();
            services.AddHttpClient<ITranslator, MyMemoryTranslateService>();
            services.AddHttpClient<ITranslator, GoogleTranslateService>();
            services.AddHttpClient<ITranslator, SystranTranslateService>();

            //-----------------
            // Recommendation: 
            // During development use XML based localization,
            // so you can benefit from AutoAddKeys and AutoTranslate options.
            // RESX based localization do not support AutoAddKeys option!
            // But we can export XML resources to RESX files using IXResourceExporter interface.
            //----------------------------------------------------------
            // Comment below line to switch to RESX based localization.
            services.AddSingleton<IXResourceProvider, XmlResourceProvider>();

            services.AddRazorPages()
                // Optional: Add {culture} route template to all razor pages routes e.g. /en/Index
                .AddRazorPagesOptions(ops => { ops.Conventions.Insert(0, new RouteTemplateModelConventionRazorPages()); })

                // Add XLocalization with a default resource <LocSource>
                // and specify a service for handling translation requests
                .AddXLocalizer<LocSource, MyMemoryTranslateService>(ops =>
                {
                    ops.ResourcesPath = "LocalizationResources";
                    ops.AutoAddKeys = true;

                    // Recommendation:
                    // To avoid extra process time/cost required by online translation,
                    // enable auto translate option after fixing texts inside the application.
                    ops.AutoTranslate = true;
                    
                    // Recommendation: 
                    // Keep caching off during development to avoid caching temporary 
                    // values that are subject to change.
                    ops.UseExpressMemoryCache = false; 
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
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
                endpoints.MapRazorPages();
            });
        }
    }
}
