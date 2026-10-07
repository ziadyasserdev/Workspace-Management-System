using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Api.Attributes;
using Workspace_Management_System.Api.Common.Responses;
using Workspace_Management_System.Application.Features.PricingRule.Commands.CreatePricingRule;
using Workspace_Management_System.Application.Features.PricingRule.Commands.DeletePricingRule;
using Workspace_Management_System.Application.Features.PricingRule.Commands.RestorePricingRule;
using Workspace_Management_System.Application.Features.PricingRule.Commands.UpdatePricingRule;
using Workspace_Management_System.Application.Features.PricingRule.Queries.GetPricingRuleById;
using Workspace_Management_System.Application.Features.PricingRule.Queries.GetPricingRules;
using Workspace_Management_System.Domain.Constants;

namespace Workspace_Management_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PricingRuleController : ControllerBase
    {
        private readonly IMediator mediator;

        public PricingRuleController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [RequirePermission(Permissions.PricingRulesCreate)]
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
            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [RequirePermission(Permissions.PricingRulesUpdate)]
        [SwaggerOperation(
            Summary = "Update pricing rule",
            Description = "Updates an existing pricing rule."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdatePricingRuleCommand command,
            CancellationToken cancellationToken)
        {
            command.Id = id;

            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [RequirePermission(Permissions.PricingRulesDelete)]
        [SwaggerOperation(
            Summary = "Delete pricing rule",
            Description = "Soft deletes an existing pricing rule."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            var command = new DeletePricingRuleCommand
            {
                Id = id
            };

            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }

        [HttpPatch("{id}/restore")]
        [RequirePermission(Permissions.PricingRulesRestore)]
        [SwaggerOperation(
            Summary = "Restore pricing rule",
            Description = "Restores a soft-deleted pricing rule."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Restore(
            int id,
            CancellationToken cancellationToken)
        {
            var command = new RestorePricingRuleCommand
            {
                Id = id
            };

            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }

        [HttpGet]
        [RequirePermission(Permissions.PricingRulesView)]
        [SwaggerOperation(
            Summary = "Get pricing rules",
            Description = "Retrieves all pricing rules."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPricingRules(
            [FromQuery] GetPricingRulesQuery query,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                query,
                cancellationToken);

            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        [RequirePermission(Permissions.PricingRulesViewDetails)]
        [SwaggerOperation(
            Summary = "Get pricing rule by ID",
            Description = "Retrieves a pricing rule by ID."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var query = new GetPricingRuleByIdQuery
            {
                Id = id
            };

            var result = await mediator.Send(
                query,
                cancellationToken);

            return result.ToActionResult();
        }
    }
}
