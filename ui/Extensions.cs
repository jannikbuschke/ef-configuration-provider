using System.Collections.Generic;
using System.Reflection;
using EfConfigurationProvider.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace EfConfigurationProvider.Ui
{
    public static class Extensions
    {
        public static IServiceCollection AddEfConfigurationUi(this IServiceCollection services, IEnumerable<Assembly> assembliesToScan)
        {
            services.AddMvcCore(options =>
            {
                options.Conventions.Add(new GenericControllerRouteConvention());
            }).ConfigureApplicationPartManager(m =>
                m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider(assembliesToScan)
            ));
            services.AddSingleton(new AssembliesCache(assembliesToScan));

            services.AddDirectoryBrowser();
            return services;
        }

        public static void UseEfConfigurationUi(this IApplicationBuilder app)
        {
            app.UseFileServer(new FileServerOptions
            {
                RequestPath = "/__configuration",
                FileProvider = new ManifestEmbeddedFileProvider(
                    assembly: Assembly.GetAssembly(typeof(Extensions)), "web/build")
            });
        }
    }
}
