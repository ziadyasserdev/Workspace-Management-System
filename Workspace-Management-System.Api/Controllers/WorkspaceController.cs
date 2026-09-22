using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Api.Common.Responses;
using Workspace_Management_System.Application.Features.Workspaces.Commands.ChangeWorkspaceStatus;
using Workspace_Management_System.Application.Features.Workspaces.Commands.CreateWorkspace;
using Workspace_Management_System.Application.Features.Workspaces.Commands.DeleteWorkspace;
using Workspace_Management_System.Application.Features.Workspaces.Commands.RestoreWorkspace;
using Workspace_Management_System.Application.Features.Workspaces.Commands.UpdateWorkspace;
using Workspace_Management_System.Application.Features.Workspaces.Queries.GetWorkspaceById;
using Workspace_Management_System.Application.Features.Workspaces.Queries.GetWorkspaces;

namespace Workspace_Management_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkspaceController : ControllerBase
    {
        private readonly IMediator mediator;

        public WorkspaceController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        [SwaggerOperation(
    Summary = "Get workspaces",
    Description = "Retrieves a paginated list of workspaces with optional search and filters."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAll(
    [FromQuery] GetWorkspacesQuery query,
    CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                query,
                cancellationToken);

            return result.ToActionResult();
        }



        [HttpPost]
        [SwaggerOperation(
    Summary = "Create workspace",
    Description = "Creates a new workspace."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create(
    [FromBody] CreateWorkspaceCommand command,
    CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }




        [HttpPut("{id:int}")]
        [SwaggerOperation(
    Summary = "Update workspace",
    Description = "Updates an existing workspace."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(
    int id,
    [FromBody] UpdateWorkspaceCommand command,
    CancellationToken cancellationToken)
        {
            command.Id = id;

            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }

        [HttpPatch("{id:int}/status")]
        [SwaggerOperation(
    Summary = "Change workspace status",
    Description = "Changes the operational status of a workspace."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ChangeStatus(
    int id,
    [FromBody] ChangeWorkspaceStatusCommand command,
    CancellationToken cancellationToken)
        {
            command.Id = id;

            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpGet("{id:int}")]
        [SwaggerOperation(
    Summary = "Get workspace by ID",
    Description = "Retrieves a workspace by its ID."
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
            var query = new GetWorkspaceByIdQuery(id);

            var result = await mediator.Send(
                query,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpDelete("{id:int}")]
        [SwaggerOperation(
    Summary = "Delete workspace",
    Description = "Soft deletes an existing workspace."
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
            var command = new DeleteWorkspaceCommand
            {
                Id = id
            };

            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpPatch("{id:int}/restore")]
        [SwaggerOperation(
    Summary = "Restore workspace",
    Description = "Restores a previously soft-deleted workspace."
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
            var command = new RestoreWorkspaceCommand
            {
                Id = id
            };

            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
    }
}
