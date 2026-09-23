using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Api.Common.Responses;
using Workspace_Management_System.Application.Features.PricingPlan.Commands.DeletePricingPlan;
using Workspace_Management_System.Application.Features.PricingPlan.Commands.RestorePricingPlan;
using Workspace_Management_System.Application.Features.PricingPlan.Commands.UpdatePricingPlan;
using Workspace_Management_System.Application.Features.PricingPlan.Queries.GetPricingPlans;

namespace Workspace_Management_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricingPlanController : ControllerBase
    {
        private readonly IMediator mediator;

        public PricingPlanController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create pricing plan",
            Description = "Creates a new pricing plan."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create(
            [FromBody] CreatePricingPlanCommand command,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpPut("{id}")]
        [SwaggerOperation(
    Summary = "Update pricing plan",
    Description = "Updates an existing pricing plan."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(
    int id,
    [FromBody] UpdatePricingPlanCommand command,
    CancellationToken cancellationToken)
        {
            command.Id = id;

            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
        Summary = "Delete pricing plan",
        Description = "Soft deletes an existing pricing plan."
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
            var command = new DeletePricingPlanCommand
            {
                Id = id
            };

            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpPatch("{id}/restore")]
        [SwaggerOperation(
    Summary = "Restore pricing plan",
    Description = "Restores a soft-deleted pricing plan."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Restore(
    int id,
    CancellationToken cancellationToken)
        {
            var command = new RestorePricingPlanCommand
            {
                Id = id
            };

            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
    
    [HttpGet]
        [SwaggerOperation(
    Summary = "Get pricing plans",
    Description = "Retrieves all pricing plans."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPricingPlans(
    CancellationToken cancellationToken)
        {
            var query = new GetPricingPlansQuery();

            var result = await mediator.Send(
                query,
                cancellationToken);

            return result.ToActionResult();
        }
    }
 }
