using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            IHostingEnvironment env = app.ApplicationServices.GetService<IHostingEnvironment>();
            //app.UseSpaStaticFiles();
            //app.UseDirectoryBrowser(new DirectoryBrowserOptions
            //{
            //    RequestPath = "/__configuration",
            //    FileProvider = new ManifestEmbeddedFileProvider(
            //        assembly: Assembly.GetAssembly(typeof(Extensions)), "web/build")
            //});

            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/__configuration",

                FileProvider = new ManifestEmbeddedFileProvider(
                    assembly: Assembly.GetAssembly(typeof(Extensions)), "web/build")
            });

            if (env.IsDevelopment())
            {
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "web";
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                });
            }
        }
    }
}
