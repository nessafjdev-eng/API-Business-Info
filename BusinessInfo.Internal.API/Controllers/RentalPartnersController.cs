using BusinessInfo.Application.Common.Models.Response;
using BusinessInfo.Application.RentalPartner.Command.Create;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusinessInfo.Internal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalPartnersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RentalPartnersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [ProducesResponseType(typeof(ResponseApiBase<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseApiBase<string>), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> CreateRentalPartnerAsync([FromBody] CreateRentalPartnerCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
