# ef-configuration-provider
An ASP.NET Core configuration provider backed by EF core

This library allows to use a sql database table for configuration values in an ASP.NET Core project.

# Install

A Nuget package is not yet available.

# Configure

in Program cs
```cs
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
```

in Startup.cs
```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddEfConfiguration();
}
```
