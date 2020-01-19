using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EfConfigurationProvider.Api
{

    [Route("api/__configuration")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly ConfigurationRoot configuration;
        private readonly ConfigurationProvider provider;
        private readonly IMediator mediator;

        public ConfigurationController(IConfiguration configuration, IMediator mediator)
        {
            this.configuration = configuration as ConfigurationRoot;
            provider = this.configuration.Providers.OfType<ConfigurationProvider>().First();
            this.mediator = mediator;
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
    }
}
