using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EfConfigurationProvider.Core;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ConfigurationProvider = EfConfigurationProvider.Core.ConfigurationProvider;

namespace EfConfigurationProvider.Api
{

    [Route("api/__configuration")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly ConfigurationRoot configuration;
        private readonly ConfigurationProvider provider;
        private readonly IMediator mediator;
        private readonly AssembliesCache assemblies;

        public ConfigurationController(IConfiguration configuration, IMediator mediator, AssembliesCache assemblies)
        {
            this.configuration = configuration as ConfigurationRoot;
            provider = this.configuration.Providers.OfType<ConfigurationProvider>().First();
            this.mediator = mediator;
            this.assemblies = assemblies;
        }

        [HttpGet("values")]
        public IDictionary<string, string> Get()
        {
            return provider.GetData();
        }

        [HttpGet("current")]
        public Configuration GetCurrentConfiguration()
        {
            return provider.GetConfiguration();
        }

        [HttpPost("update")]
        public async Task<ActionResult> Update(Update request)
        {
            try
            {
                await mediator.Send(request);
                return Ok();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpGet("partial-configurations")]
        public IEnumerable<GeneratedControllerAttribute> GetPartialConfigurations()
        {
            IEnumerable<GeneratedControllerAttribute> attributes = assemblies
                .SelectMany(v => v.GetExportedTypes().Where(v => v.GetCustomAttributes(typeof(GeneratedControllerAttribute), true).Any())
                    .SelectMany(v => v.GetCustomAttributes<GeneratedControllerAttribute>())
                );

            return attributes;
        }
    }
}
