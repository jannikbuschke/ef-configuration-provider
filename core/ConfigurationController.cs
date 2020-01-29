using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EfConfigurationProvider.Core
{

    [Route("api/__configuration")]
    [ApiController]
    [Authorize, AllowAnonymous]
    public class ConfigurationController : ControllerBase
    {
        private readonly ConfigurationRoot configuration;
        private readonly ConfigurationProvider provider;
        private readonly IMediator mediator;
        private readonly AssembliesCache assemblies;
        private readonly AuthorizationService authorizationService;

        public ConfigurationController(IConfiguration configuration, IMediator mediator, AssembliesCache assemblies, AuthorizationService authorizationService)
        {
            this.configuration = configuration as ConfigurationRoot;
            provider = this.configuration.Providers.OfType<ConfigurationProvider>().First();
            this.mediator = mediator;
            this.assemblies = assemblies;
            this.authorizationService = authorizationService;
        }

        [HttpGet("values")]
        public async Task<ActionResult<IDictionary<string, string>>> Get()
        {
            if (await authorizationService.ReadAllAllowed())
            {
                return Ok(provider.GetData());
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("current")]
        public async Task<ActionResult<Configuration>> GetCurrentConfiguration()
        {
            if (await authorizationService.ReadAllAllowed())
            {
                return Ok(provider.GetConfiguration());
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult> Update(Update request)
        {
            if (!await authorizationService.UpdateAllAllowed())
            {
                return Unauthorized();
            }

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
