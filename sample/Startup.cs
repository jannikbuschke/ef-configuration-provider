using System.Collections.Generic;
using EfConfigurationProvider.Api;
using EfConfigurationProvider.Ui;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace EfConfigurationProvider
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration config)
        {
            configuration = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Conventions.Add(new GenericControllerRouteConvention());
            }).ConfigureApplicationPartManager(m =>
                m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider(GetType().Assembly)
            ));
            services.AddEfConfiguration();
            services.AddEfConfigurationUi();

            services.Configure<StronglyTypedOptions>(configuration.GetSection("strongly-typed-options"));
            services.Configure<StronglyTypedOptions2>(configuration.GetSection("strongly-typed-options-2"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseEfConfigurationUi();

            if (env.IsDevelopment())
            {
                app.Run(async (context) =>
                {
                    IEnumerable<KeyValuePair<string, string>> data = configuration.AsEnumerable();

                    await context.Response.WriteAsync(JArray.FromObject(data).ToString());
                });
            }
        }
    }
}
