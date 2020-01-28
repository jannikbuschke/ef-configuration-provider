using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EfConfigurationProvider.Core.Sqlite
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlite("Data Source=sqlite.sqlite", options =>
             {
                 options.MigrationsAssembly(GetType().Assembly.FullName);
             });

            return new DataContext(optionsBuilder.Options);
        }
    }
}
