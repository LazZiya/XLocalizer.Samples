using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using XmlLocalizationSample.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using XLocalizer;
using XLocalizer.Xml;
using XLocalizer.Routing;
using XLocalizer.Translate;
using XLocalizer.Translate.MyMemoryTranslate;
using XLocalizer.Translate.GoogleTranslate;
using XLocalizer.Translate.SystranTranslate;
using XLocalizer.Translate.YandexTranslate;
using XmlLocalizationSample.LocalizationResources;
using System;

namespace XmlLocalizationSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<RequestLocalizationOptions>(ops =>
            {
                var cultures = new CultureInfo[] { new CultureInfo("en"), new CultureInfo("tr"), new CultureInfo("ar"), new CultureInfo("de") };
                ops.SupportedCultures = cultures;
                ops.SupportedUICultures = cultures;
                ops.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");

                // Optional: add custom provider to support localization based on route value {culture}
                ops.RequestCultureProviders.Insert(0, new RouteSegmentRequestCultureProvider(cultures));
            });

            // Optional: To enable online translation register one or more translation services.
            // Then add API Keys to user secrets file.
            // For more details see: http://docs.ziyad.info/XLocalizer/translate-services.md
            // services.AddHttpClient<ITranslator, YandexTranslateService>();
            // services.AddHttpClient<ITranslator, GoogleTranslateService>();
            // services.AddHttpClient<ITranslator, SystranTranslateService>();
            services.AddHttpClient<ITranslator, MyMemoryTranslateService>();

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
                    ops.AutoAddKeys = false;
                    ops.AutoTranslate = false;
                    ops.UseExpressMemoryCache = false;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            // Use request localization middleware
            app.UseRequestLocalization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
