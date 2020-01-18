using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EfConfigurationProvider
{
    public static class EntityFrameworkExtensions
    {
        internal static Action<DbContextOptionsBuilder> optionsAction;
        public static IConfigurationBuilder AddEFConfiguration(
            this IConfigurationBuilder builder,
            Action<DbContextOptionsBuilder> optionsAction)
        {
            EntityFrameworkExtensions.optionsAction = optionsAction;
            return builder.Add(new ConfigurationSource(optionsAction));
        }

        public static IServiceCollection AddEfConfiguration(this IServiceCollection services)
        {
            services.AddMediatR();
            return services;
        }
    }
}
