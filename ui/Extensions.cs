using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace EfConfigurationProvider.Ui
{
    public static class Extensions
    {
        public static IServiceCollection AddEfConfigurationUi(this IServiceCollection services)
        {
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "web/build";
            });
            services.AddDirectoryBrowser();
            return services;
        }

        public static void UseEfConfigurationUi(this IApplicationBuilder app)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/__configuration",

                FileProvider = new ManifestEmbeddedFileProvider(
                    assembly: Assembly.GetAssembly(typeof(Extensions)), "web/build")
            });
        }
    }
}
