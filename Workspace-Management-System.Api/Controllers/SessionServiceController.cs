using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Api.Attributes;
using Workspace_Management_System.Application.Features.Sessions.Services.Commands.AddService;
using Workspace_Management_System.Application.Features.Sessions.Services.Commands.ClearServices;
using Workspace_Management_System.Application.Features.Sessions.Services.Commands.DeleteService;
using Workspace_Management_System.Application.Features.Sessions.Services.Commands.UpdateService;
using Workspace_Management_System.Application.Features.Sessions.Services.Dtos;
using Workspace_Management_System.Application.Features.Sessions.Services.Queries.GetSessionServiceById;
using Workspace_Management_System.Application.Features.Sessions.Services.Queries.GetSessionServices;
using Workspace_Management_System.Domain.Constants;

namespace Workspace_Management_System.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SessionServiceController : ControllerBase
{
    private readonly IMediator _mediator;

    public SessionServiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("sessions/{sessionId}/services")]
    [RequirePermission(Permissions.SessionServicesAdd)]
    [SwaggerOperation(
        Summary = "Add a service to an active session",
        Description = "Adds a service with a specified quantity to an active session.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddService(
        int sessionId,
        [FromBody] AddServiceRequestDto request)
    {
        var command = new AddServiceCommand
        {
            SessionId = sessionId,
            ServiceId = request.ServiceId,
            Quantity = request.Quantity
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("sessions/{sessionId}/services/{serviceId}")]
    [RequirePermission(Permissions.SessionServicesUpdate)]
    [SwaggerOperation(
        Summary = "Update a service in an active session",
        Description = "Updates the quantity of a service already added to an active session.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateService(
        int sessionId,
        int serviceId,
        [FromBody] UpdateServiceRequestDto request)
    {
        var command = new UpdateServiceCommand
        {
            SessionId = sessionId,
            ServiceId = serviceId,
            Quantity = request.Quantity
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpDelete("sessions/{sessionId}/services/{serviceId}")]
    [RequirePermission(Permissions.SessionServicesRemove)]
    [SwaggerOperation(
        Summary = "Delete a service from an active session",
        Description = "Removes a specific service from an active session.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteService(
        int sessionId,
        int serviceId)
    {
        var command = new DeleteServiceCommand
        {
            SessionId = sessionId,
            ServiceId = serviceId
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpDelete("sessions/{sessionId}/services")]
    [RequirePermission(Permissions.SessionServicesClear)]
    [SwaggerOperation(
        Summary = "Clear all services from an active session",
        Description = "Removes all services from an active session.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ClearServices(int sessionId)
    {
        var command = new ClearServicesCommand
        {
            SessionId = sessionId
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpGet("sessions/{sessionId}/services")]
    [RequirePermission(Permissions.SessionServicesView)]
    [SwaggerOperation(
        Summary = "Get session services",
        Description = "Gets all services added to a session.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSessionServices(int sessionId)
    {
        var result = await _mediator.Send(
            new GetSessionServicesQuery
            {
                SessionId = sessionId
            });

        return Ok(result);
    }

    [HttpGet("services/{id}")]
    [RequirePermission(Permissions.SessionServicesViewDetails)]
    [SwaggerOperation(
        Summary = "Get session service by ID",
        Description = "Gets a specific service added to a session.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSessionServiceById(int id)
    {
        var result = await _mediator.Send(
            new GetSessionServiceByIdQuery
            {
                Id = id
            });

        return Ok(result);
    }
}
