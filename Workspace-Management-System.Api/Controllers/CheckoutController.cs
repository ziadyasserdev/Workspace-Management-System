using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Application.Features.Checkout.Commands.CheckoutSession;
using Workspace_Management_System.Application.Features.Checkout.Dtos;

namespace Workspace_Management_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CheckoutController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("sessions/{sessionId:int}")]
        [SwaggerOperation(
            Summary = "Checkout session",
            Description = "Checks out an active session, calculates the workspace and product charges, creates a transaction, and closes the session."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Checkout(
            int sessionId,
            [FromBody] CheckoutRequestDto request,
            CancellationToken cancellationToken)
        {
            var command = new CheckoutSessionCommand
            {
                SessionId = sessionId,
                Request = request
            };

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return Ok(result);
        }
    }
}