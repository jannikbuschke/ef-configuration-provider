using System.Collections.Generic;
using EfConfigurationProvider.Core;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EfConfigurationProvider.Sample
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
            services.AddAuthorization(options =>
            {
                options.AddPolicy("policy", policy =>
                {
                    policy.RequireRole("Everyone");
                });
            });
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddGlowConfiguration(
                new[] { typeof(Startup).Assembly },
                options =>
            {
                //options.GlobalPolicy = "policy";
            });

            services.Configure<StronglyTypedOptions>(configuration.GetSection("strongly-typed-options"));
            services.Configure<StronglyTypedOptions2>(configuration.GetSection("strongly-typed-options-2"));

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "web/build";
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            //app.UseEfConfigurationUi();

            //if (env.IsDevelopment())
            //{
            //    app.Run(async (context) =>
            //    {
            //        IEnumerable<KeyValuePair<string, string>> data = configuration.AsEnumerable();

            //        await context.Response.WriteAsync(JArray.FromObject(data).ToString());
            //    });
            //}

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "web";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                }
            });
        }
    }
}
