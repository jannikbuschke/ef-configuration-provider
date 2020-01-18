using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace EfConfigurationProvider
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, conf) =>
                {
                    var cs = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=config-test;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                    conf.AddEFConfiguration(options => options.UseSqlServer(cs));
                })
                .UseStartup<Startup>();
        }
    }
}
