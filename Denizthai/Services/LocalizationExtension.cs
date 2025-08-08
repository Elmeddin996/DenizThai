using Microsoft.AspNetCore.Localization;
using Denizthai.Resources;
using System.Globalization;
using System.Reflection;

namespace Denizthai.Services
{
    public static class LocalizationExtension
    {
        public static void AddLocalizationService(this IServiceCollection services)
        {
            services.AddSingleton<LocService>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);

            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddViewLocalization()
            .AddDataAnnotationsLocalization(
                options =>
                  options.DataAnnotationLocalizerProvider = (type, factory) =>
                  {

                      return factory.Create("SharedResource", assemblyName.Name);
                  }
                );
        }

        public static void AddLocalization(this WebApplication app)
        {

            var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("ru"), new CultureInfo("az") };

            app.UseRequestLocalization(
              new RequestLocalizationOptions()
              {
                  DefaultRequestCulture = new RequestCulture("en"),
                  SupportedCultures = supportedCultures,
                  SupportedUICultures = supportedCultures
              }
            );

        }
    }
}
