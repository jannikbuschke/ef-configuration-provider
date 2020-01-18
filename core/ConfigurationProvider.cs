using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EfConfigurationProvider
{

    public class ConfigurationProvider : Microsoft.Extensions.Configuration.ConfigurationProvider, IConfigurationProvider
    {
        public static ConfigurationProvider Value { private set; get; }

        public ConfigurationProvider(Action<DbContextOptionsBuilder> optionsAction)
        {
            OptionsAction = optionsAction;
            ConfigurationProvider.Value = this;

            var builder = new DbContextOptionsBuilder<DataContext>();

            OptionsAction(builder);

            using var dbContext = new DataContext(builder.Options);
            dbContext.Database.Migrate();
        }

        private Action<DbContextOptionsBuilder> OptionsAction { get; }

        public void Reload()
        {
            Load();
            OnReload();
        }

        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();

            OptionsAction(builder);

            using var dbContext = new DataContext(builder.Options);

            Data = dbContext.Values.ToDictionary(c => c.Name, c => c.Value);
        }

        public IDictionary<string, string> GetData()
        {
            return Data;
        }
    }
}
