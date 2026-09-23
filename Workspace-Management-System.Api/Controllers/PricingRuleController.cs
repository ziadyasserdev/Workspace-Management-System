using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Api.Common.Responses;
using Workspace_Management_System.Application.Features.PricingRule.Commands.CreatePricingRule;

namespace Workspace_Management_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricingRuleController : ControllerBase
    {
        public IMediator Mediator { get; set; }

        public PricingRuleController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create pricing rule",
            Description = "Creates a new pricing rule."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create(
            [FromBody] CreatePricingRuleCommand command,
            CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
    }
}