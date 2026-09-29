using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Application.Features.Sessions.Services.Commands.AddService;
using Workspace_Management_System.Application.Features.Sessions.Services.Commands.ClearServices;
using Workspace_Management_System.Application.Features.Sessions.Services.Commands.DeleteService;
using Workspace_Management_System.Application.Features.Sessions.Services.Commands.UpdateService;
using Workspace_Management_System.Application.Features.Sessions.Services.Dtos;

namespace Workspace_Management_System.Api.Controllers;

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
    [SwaggerOperation(
        Summary = "Add a service to an active session",
        Description = "Adds a service with a specified quantity to an active session.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    [SwaggerOperation(
    Summary = "Update a service in an active session",
    Description = "Updates the quantity of a service already added to an active session.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    [SwaggerOperation(
Summary = "Delete a service from an active session",
Description = "Removes a specific service from an active session.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
   

}
