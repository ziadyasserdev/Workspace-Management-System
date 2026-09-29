using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using Workspace_Management_System.Api.Common.Responses;
using Workspace_Management_System.Api.Controllers;
using Workspace_Management_System.Application.Features.Services.Commands.CreateService;
using Workspace_Management_System.Application.Features.Services.Commands.DeleteService;
using Workspace_Management_System.Application.Features.Services.Commands.RestoreService;
using Workspace_Management_System.Application.Features.Services.Commands.UpdateService;
using Workspace_Management_System.Application.Features.Services.Queries.GetServiceById;
using Workspace_Management_System.Application.Features.Services.Queries.GetServices;
using static System.Net.WebRequestMethods;

namespace Workspace_Management_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IMediator mediator;

        public ServiceController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create service",
            Description = "Creates a new service."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create(
            [FromBody] CreateServiceCommand command,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);

            return result.ToActionResult();
        }
        [HttpPut("{id:int}")]
        [SwaggerOperation(
    Summary = "Update service",
    Description = "Updates an existing service."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(
    int id,
    [FromBody] UpdateServiceCommand command,
    CancellationToken cancellationToken)
        {
            command.Id = id;

            var result = await mediator.Send(command, cancellationToken);

            return result.ToActionResult();
        }

        [HttpDelete("{id:int}")]
        [SwaggerOperation(
            Summary = "Delete service",
            Description = "Soft deletes an existing service."
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
                    var command = new DeleteServiceCommand
                    {
                        Id = id
                    };

                    var result = await mediator.Send(command, cancellationToken);

                    return result.ToActionResult();
        }

        [HttpPatch("{id:int}/restore")]
        [SwaggerOperation(
    Summary = "Restore service",
    Description = "Restores a previously deleted service."
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
            var command = new RestoreServiceCommand
            {
                Id = id
            };

            var result = await mediator.Send(command, cancellationToken);

            return result.ToActionResult();
        }


        [HttpGet]
        [SwaggerOperation(
            Summary = "Get services",
            Description = "Returns all non-deleted services with optional search, active status filter, and pagination."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
                {
                    var query = new GetServicesQuery
                    {
                        Search = search,
                        IsActive = isActive,
                        PageNumber = pageNumber,
                        PageSize = pageSize
                    };

                    var result = await mediator.Send(query, cancellationToken);

                    return result.ToActionResult();
        }

        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Get service by ID",
            Description = "Returns a service by its ID."
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
                    var query = new GetServiceByIdQuery
                    {
                        Id = id
                    };

                    var result = await mediator.Send(query, cancellationToken);

                    return result.ToActionResult();
                }


    }
}
