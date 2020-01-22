using EfConfigurationProvider.Core;
using EfConfigurationProvider.Ui;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace EfConfigurationProvider.Sample
{
    public class Startup
    {
        private readonly IConfiguration config;

        public Startup(IConfiguration config)
        {
            this.config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
            });
            services.AddEfConfiguration();
            services.AddEfConfigurationUi();
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
                    System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> data = config.AsEnumerable();

                    await context.Response.WriteAsync(JArray.FromObject(data).ToString());
                });
            }
        }
    }
}
