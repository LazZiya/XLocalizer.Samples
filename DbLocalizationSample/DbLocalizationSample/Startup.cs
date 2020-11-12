using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using DBLocalizationSample.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XLocalizer.Translate.MyMemoryTranslate;
using XLocalizer.Translate;
using System.Globalization;
using XLocalizer.Routing;
using XLocalizer.DB;
using XLocalizer.Translate.YandexTranslate;
using XLocalizer.Translate.GoogleTranslate;
using XLocalizer.Translate.SystranTranslate;

namespace DBLocalizationSample
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
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")),
                    ServiceLifetime.Transient,
                    ServiceLifetime.Transient);
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<RequestLocalizationOptions>(ops =>
            {
                var cultures = new CultureInfo[] { new CultureInfo("en"), new CultureInfo("tr"), new CultureInfo("ar") };
                ops.SupportedCultures = cultures;
                ops.SupportedUICultures = cultures;
                ops.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");
                ops.RequestCultureProviders.Insert(0, new RouteSegmentRequestCultureProvider(cultures));
            });

            // Optional: To enable online translation register one or more translation services.
            // Then add API Keys to user secrets file.
            // For more details see: https://docs.ziyad.info/en/XLocalizer/v1.0/translate-services.md
            // uncomment the services that you want to use for manually translation
            services.AddHttpClient<ITranslator, MyMemoryTranslateService>();
            //services.AddHttpClient<ITranslator, MyMemoryTranslateServiceRapidApi>();
            //services.AddHttpClient<ITranslator, GoogleTranslateService>();
            //services.AddHttpClient<ITranslator, GoogleTranslateServiceRapidApi>();
            //services.AddHttpClient<ITranslator, MicrosoftTranslateService>();
            //services.AddHttpClient<ITranslator, MicrosoftTranslateServiceRapidApi>();
            //services.AddHttpClient<ITranslator, SystranTranslateServiceRapidApi>();
            //services.AddHttpClient<ITranslator, YandexTranslateService>();

            services.AddRazorPages()
                .AddRazorPagesOptions(ops => { ops.Conventions.Insert(0, new RouteTemplateModelConventionRazorPages()); })
                .AddXDbLocalizer<ApplicationDbContext, MyMemoryTranslateService>(ops =>
                {
                    ops.AutoAddKeys = true;
                    ops.AutoTranslate = true;
                    ops.UseExpressMemoryCache = true;
                });
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
