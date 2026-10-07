using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Api.Common.Responses;
using Workspace_Management_System.Application.Features.Authentications.Queries.GetCurrentUserPermissions;
using Workspace_Management_System.Application.Features.Authentications.Queries.GetRolePermissions;
using Workspace_Management_System.Domain.Constants;


namespace Workspace_Management_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator mediator;

        public PermissionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize]
        [HttpGet("current")]
        [SwaggerOperation(
            Summary = "Get current user permissions",
            Description = "Return the authenticated user's roles and permissions."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCurrentUserPermissions()
        {
            var result = await mediator.Send(
                new GetCurrentUserPermissionsQuery());

            return result.ToActionResult();
        }

        [Authorize(Roles = $"{Roles.Owner},{Roles.Admin}")]
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all role permissions",
            Description = "Return all roles and their assigned permissions."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetRolePermissions()
        {
            var result = await mediator.Send(
                new GetRolePermissionsQuery());

            return result.ToActionResult();
        }
    }
}