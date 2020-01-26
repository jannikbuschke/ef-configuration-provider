using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EfConfigurationProvider.Core
{
    public class ConfigurationSource : IConfigurationSource
    {
        private readonly Action<DbContextOptionsBuilder> optionsAction;

        public ConfigurationSource(Action<DbContextOptionsBuilder> optionsAction)
        {
            this.optionsAction = optionsAction;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ConfigurationProvider(optionsAction);
        }
    }
}
