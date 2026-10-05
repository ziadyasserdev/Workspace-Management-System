using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Api.Common.Responses;
using Workspace_Management_System.Application.Features.Memberships.Commands.ChangeMembershipStatus;
using Workspace_Management_System.Application.Features.Memberships.Commands.CreateMembership;
using Workspace_Management_System.Application.Features.Memberships.Commands.DeleteMembership;
using Workspace_Management_System.Application.Features.Memberships.Commands.UpdateMembership;
using Workspace_Management_System.Application.Features.Memberships.Queries.GetMembershipById;
using Workspace_Management_System.Application.Features.Memberships.Queries.GetMemberships;

namespace Workspace_Management_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MembershipController(IMediator _mediator)
        {
           this. _mediator = _mediator;
        }
        [HttpPost]
        [SwaggerOperation(
    Summary = "Create a membership",
    Description = "Creates a new active membership plan."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateMembership(
    [FromBody] CreateMembershipCommand command,
    CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpGet]
        [SwaggerOperation(
    Summary = "Get memberships",
    Description = "Returns a paginated list of memberships with optional search and active status filtering."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMemberships(
    [FromQuery] GetMembershipsQuery query,
    CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                query,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpGet("{id:int}")]
        [SwaggerOperation(
    Summary = "Get membership by ID",
    Description = "Returns membership details including its active benefits."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMembershipById(
    int id,
    CancellationToken cancellationToken)
        {
            var query = new GetMembershipByIdQuery
            {
                Id = id
            };

            var result = await _mediator.Send(
                query,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpPut("{id:int}")]
        [SwaggerOperation(
    Summary = "Update membership",
    Description = "Updates membership information without changing its active status."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateMembership(
    int id,
    [FromBody] UpdateMembershipCommand command,
    CancellationToken cancellationToken)
        {
            command.Id = id;

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpPatch("{id:int}/status")]
        [SwaggerOperation(
    Summary = "Change membership status",
    Description = "Activates or deactivates a membership plan."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ChangeMembershipStatus(
    int id,
    [FromBody] ChangeMembershipStatusCommand command,
    CancellationToken cancellationToken)
        {
            command.Id = id;

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpDelete("{id:int}")]
        [SwaggerOperation(
    Summary = "Delete membership",
    Description = "Soft deletes a membership if it has no active customer subscriptions."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteMembership(
    int id,
    CancellationToken cancellationToken)
        {
            var command = new DeleteMembershipCommand
            {
                Id = id
            };

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
    }
}
