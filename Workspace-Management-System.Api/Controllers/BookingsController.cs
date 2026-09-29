using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Api.Common.Responses;
using Workspace_Management_System.Application.Features.Bookings.Commands.ChangeBookingStatus;
using Workspace_Management_System.Application.Features.Bookings.Commands.CreateBooking;
using Workspace_Management_System.Application.Features.Bookings.Commands.DeleteBooking;
using Workspace_Management_System.Application.Features.Bookings.Commands.UpdateBooking;
using Workspace_Management_System.Application.Features.Bookings.Queries.GetBookingById;
using Workspace_Management_System.Application.Features.Bookings.Queries.GetBookings;

namespace Workspace_Management_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        [HttpPost]
        [SwaggerOperation(
    Summary = "Create a new booking",
    Description = "Creates a pending booking after validating customer, workspace, capacity, time, and booking conflicts."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateBooking(
    [FromBody] CreateBookingCommand command,
    CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpGet]
        [SwaggerOperation(
    Summary = "Get bookings",
    Description = "Returns bookings with pagination, search, filters, date range, and sorting."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetBookings(
    [FromQuery] GetBookingsQuery query,
    CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                query,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpGet("{id:int}")]
        [SwaggerOperation(
    Summary = "Get booking by id",
    Description = "Returns detailed information about a booking including customer, workspace, and session information."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookingById(
    int id,
    CancellationToken cancellationToken)
        {
            var query = new GetBookingByIdQuery
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
    Summary = "Update booking",
    Description = "Updates a pending or confirmed booking after validating customer, workspace, capacity, time, and booking conflicts."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateBooking(
    int id,
    [FromBody] UpdateBookingCommand command,
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
    Summary = "Change booking status",
    Description = "Changes a booking status according to the allowed booking lifecycle transitions."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ChangeBookingStatus(
    int id,
    [FromBody] ChangeBookingStatusCommand command,
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
    Summary = "Delete a booking",
    Description = "Soft deletes a booking after validating its current state and session relationship."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteBooking(
    int id,
    CancellationToken cancellationToken)
        {
            var command = new DeleteBookingCommand
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
