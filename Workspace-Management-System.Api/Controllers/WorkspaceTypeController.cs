using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Api.Common.Responses;
using Workspace_Management_System.Application.Features.WorkspaceTypes.Commands.ChangeWorkspaceTypeStatus;
using Workspace_Management_System.Application.Features.WorkspaceTypes.Commands.CreateWorkspaceType;
using Workspace_Management_System.Application.Features.WorkspaceTypes.Commands.UpdateWorkspaceType;
using Workspace_Management_System.Application.Features.WorkspaceTypes.Queries.GetWorkspaceTypeById;
using Workspace_Management_System.Application.Features.WorkspaceTypes.Queries.GetWorkspaceTypes;
using Workspace_Management_System.Domain.Constants;

namespace Workspace_Management_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkspaceTypeController : ControllerBase
    {
        private readonly IMediator mediator;

        public WorkspaceTypeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

       // [Authorize(Policy = Permissions.WorkspaceTypesCreate)]
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create workspace type",
            Description = "Creates a new workspace type."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create(
            [FromBody] CreateWorkspaceTypeCommand command,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);

            return result.ToActionResult();
        }
        [HttpGet("{id:int}")]
        [SwaggerOperation(
    Summary = "Get workspace type by ID",
    Description = "Retrieves a workspace type by its ID."
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
            var query = new GetWorkspaceTypeByIdQuery(id);

            var result = await mediator.Send(
                query,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpGet]
        [SwaggerOperation(
    Summary = "Get workspace types",
    Description = "Retrieves a paginated list of workspace types with optional search and active status filtering."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAll(
    [FromQuery] GetWorkspaceTypesQuery query,
    CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                query,
                cancellationToken);

            return result.ToActionResult();
        }

        [HttpPut("{id:int}")]
        [SwaggerOperation(
    Summary = "Update workspace type",
    Description = "Updates an existing workspace type."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(
    int id,
    [FromBody] UpdateWorkspaceTypeCommand command,
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
    Summary = "Change workspace type status",
    Description = "Activates or deactivates an existing workspace type."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeStatus(
    int id,
    [FromBody] ChangeWorkspaceTypeStatusCommand command,
    CancellationToken cancellationToken)
        {
            command.Id = id;

            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
    }
}
