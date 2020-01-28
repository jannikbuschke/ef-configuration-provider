using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EfConfigurationProvider.Core.SqlServerMigrations
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer("no-value", options =>
             {
                 options.MigrationsAssembly(GetType().Assembly.FullName);
             });

            return new DataContext(optionsBuilder.Options);
        }
    }
}
