using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EfConfigurationProvider.Core
{
    [Route("api/config")]
    [Authorize, AllowAnonymous]
    public class PartialUpdateController<T> : ControllerBase where T : class, new()
    {
        private readonly IMediator mediator;

        public PartialUpdateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public T Get([FromServices] IOptionsSnapshot<T> options)
        {
            return options.Value;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PartialUpdateRaw<T> value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await mediator.Send(value.ToPartialUpdate());
            return Ok();
        }
    }
}
