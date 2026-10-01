using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Api.Common.Responses;
using Workspace_Management_System.Application.Features.MembershipBenefits.Commands.CreateMembershipBenefit;
using Workspace_Management_System.Application.Features.MembershipBenefits.Commands.DeleteMembershipBenefit;
using Workspace_Management_System.Application.Features.MembershipBenefits.Commands.UpdateMembershipBenefit;
using Workspace_Management_System.Application.Features.MembershipBenefits.Queries.GetMembershipBenefitById;
using Workspace_Management_System.Application.Features.MembershipBenefits.Queries.GetMembershipBenefits;

namespace Workspace_Management_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipBenefitsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MembershipBenefitsController(IMediator _mediator)
        {
           this. _mediator = _mediator;
        }
        [HttpPost("{membershipId:int}/benefits")]
        [SwaggerOperation(
    Summary = "Create membership benefit",
    Description = "Adds a new benefit to an existing membership."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateBenefit(
    int membershipId,
    [FromBody] CreateMembershipBenefitCommand command,
    CancellationToken cancellationToken)
        {
            command.MembershipId = membershipId;

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpGet("{membershipId:int}/benefits")]
        [SwaggerOperation(
    Summary = "Get membership benefits",
    Description = "Returns all active benefits belonging to a membership."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMembershipBenefits(
    int membershipId,
    CancellationToken cancellationToken)
        {
            var query = new GetMembershipBenefitsQuery
            {
                MembershipId = membershipId
            };

            var result = await _mediator.Send(
                query,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpGet("{membershipId:int}/benefits/{benefitId:int}")]
        [SwaggerOperation(
    Summary = "Get membership benefit by ID",
    Description = "Returns a specific benefit belonging to the specified membership."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMembershipBenefitById(
    int membershipId,
    int benefitId,
    CancellationToken cancellationToken)
        {
            var query = new GetMembershipBenefitByIdQuery
            {
                MembershipId = membershipId,
                BenefitId = benefitId
            };

            var result = await _mediator.Send(
                query,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpPut("{membershipId:int}/benefits/{benefitId:int}")]
        [SwaggerOperation(
    Summary = "Update membership benefit",
    Description = "Updates a benefit belonging to the specified membership."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateMembershipBenefit(
    int membershipId,
    int benefitId,
    [FromBody] UpdateMembershipBenefitCommand command,
    CancellationToken cancellationToken)
        {
            command.MembershipId = membershipId;
            command.BenefitId = benefitId;

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpDelete("{membershipId:int}/benefits/{benefitId:int}")]
        [SwaggerOperation(
    Summary = "Delete membership benefit",
    Description = "Soft deletes a benefit belonging to the specified membership."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMembershipBenefit(
    int membershipId,
    int benefitId,
    CancellationToken cancellationToken)
        {
            var command = new DeleteMembershipBenefitCommand
            {
                MembershipId = membershipId,
                BenefitId = benefitId
            };

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
    }
}
