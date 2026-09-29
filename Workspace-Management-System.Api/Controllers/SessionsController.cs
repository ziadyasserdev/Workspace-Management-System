using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Api.Common.Responses;
using Workspace_Management_System.Application.Features.Sessions.Commands.ChangeSessionWorkspace;
using Workspace_Management_System.Application.Features.Sessions.Commands.EndSession;
using Workspace_Management_System.Application.Features.Sessions.Commands.StartSession;
using Workspace_Management_System.Application.Features.Sessions.Queries.GetActiveSessions;
using Workspace_Management_System.Application.Features.Sessions.Queries.GetSessionDetails;

namespace Workspace_Management_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SessionsController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Start a new session",
            Description = "Starts a customer session and marks the workspace as occupied."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> StartSession(
            [FromBody] StartSessionCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpGet("active")]
        [SwaggerOperation(
    Summary = "Get active sessions",
    Description = "Returns all currently active sessions with pagination and optional filters."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetActiveSessions(
    [FromQuery] GetActiveSessionsQuery query,
    CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                query,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpGet("{id:int}")]
        [SwaggerOperation(
    Summary = "Get session details",
    Description = "Returns session details including workspace history."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSessionDetails(
    int id,
    CancellationToken cancellationToken)
        {
            var query = new GetSessionDetailsQuery
            {
                Id = id
            };

            var result = await _mediator.Send(
                query,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpPost("{id:int}/end")]
        [SwaggerOperation(
    Summary = "End a session",
    Description = "Ends an active session and makes its workspace available."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> EndSession(
    int id,
    CancellationToken cancellationToken)
        {
            var command = new EndSessionCommand
            {
                SessionId = id
            };

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpPatch("{id:int}/workspace")]
        [SwaggerOperation(
    Summary = "Change session workspace",
    Description = "Moves an active session to another available workspace."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ChangeWorkspace(
    int id,
    [FromBody] ChangeSessionWorkspaceCommand command,
    CancellationToken cancellationToken)
        {
            command.SessionId = id;

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
    }
}
