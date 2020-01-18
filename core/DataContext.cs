using Microsoft.EntityFrameworkCore;

namespace EfConfigurationProvider
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ConfigurationValue> Values { get; set; }
    }
}
