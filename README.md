# ef-configuration-provider
An ASP.NET Core configuration provider backed by EF core

This library allows to use a sql database table for configuration values in an ASP.NET Core project.

# Install

https://www.nuget.org/packages/EfConfigurationProvider/


```
dotnet add package EfConfigurationProvider
```

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
    services.AddMvc();
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
    app.UseEfConfigurationUi(); // enables a ui served under  /__configuration/index.html
}
```

# Features / Roadmap

 - :white_check_mark: Simple table based edit of all key/values
 - :white_check_mark: MS SQL provider
 - :white_check_mark: Embedded frontend
 - :white_check_mark: Save history
 - :black_square_button: Allow rollbacks
 - :black_square_button: Configurable authorization
 - :black_square_button: Configurable routes (for frontend and backend)
 - :black_square_button: Postgresql, MySQL, SQLite provider
