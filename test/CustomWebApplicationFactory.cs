using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace EfConfigurationProvider.Test
{

    public class CustomWebApplicationFactory<TEntryPoint>
      : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        static CustomWebApplicationFactory()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "IntegrationTest");
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (!string.IsNullOrEmpty(environment))
            {
                builder.UseEnvironment(environment);
            }
            var cs = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=config-test;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            var options = new DbContextOptionsBuilder { };
            options.UseSqlServer(cs);
            using var dbContext = new DataContext(options.Options);
            //dbContext.Database.EnsureDeleted();
            dbContext.Database.Migrate();

            ConfigurationValue val = dbContext.Values.SingleOrDefault(v => v.Name == "cs");
            if (val == null)
            {
                dbContext.Values.Add(new ConfigurationValue
                {
                    Name = "cs",
                    Value = "value123"
                });
                dbContext.Values.Add(new ConfigurationValue
                {
                    Name = "key5",
                    Value = "value5"
                });
                dbContext.SaveChanges();
            }

            builder.ConfigureAppConfiguration((context, conf) =>
            {
                conf.AddEFConfiguration(options => options.UseSqlServer(cs));
            });
        }
    }
}
