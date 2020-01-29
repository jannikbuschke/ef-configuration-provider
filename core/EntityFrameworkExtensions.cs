using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EfConfigurationProvider.Core
{
    public class Options
    {
        private readonly Dictionary<string, string> partialReadPolicies = new Dictionary<string, string>();
        private readonly Dictionary<string, string> partialWritePolicies = new Dictionary<string, string>();

        public string GlobalPolicy { get; set; }
        public string ReadAllPolicy { get; set; }
        public string WriteAllPolicy { get; set; }

        public void SetPartialReadPolicy(string path, string policy)
        {
            partialReadPolicies[path] = policy;
        }

        public string GetPartialReadPolicy(string path)
        {
            return partialReadPolicies.GetValueOrDefault(path);
        }

        public void SetPartialWritePolicy(string path, string policy)
        {
            partialWritePolicies[path] = policy;
        }

        public string GetWriteReadPolicy(string path)
        {
            return partialWritePolicies.GetValueOrDefault(path);
        }
    }

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

        public static IServiceCollection AddEfConfiguration(this IServiceCollection services, Action<Options> configure)
        {
            var configuration = new Options();
            configure(configuration);
            services.AddMediatR();
            services.AddSingleton(configuration);
            services.AddSingleton<AuthorizationService>();
            services.AddHttpContextAccessor();
            return services;
        }

        public static IServiceCollection AddEfConfiguration(this IServiceCollection services)
        {
            return AddEfConfiguration(services, options => { });
        }
    }
}
