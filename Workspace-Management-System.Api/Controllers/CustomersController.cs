using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Api.Common.Responses;
using Workspace_Management_System.Application.Features.Customers.Commands.CreateCustomer;
using Workspace_Management_System.Application.Features.Customers.Commands.Delete_Customer;
using Workspace_Management_System.Application.Features.Customers.Commands.UpdateCustomer;
using Workspace_Management_System.Application.Features.Customers.Queries.SearchCustomers;

namespace Workspace_Management_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator mediator;

        public CustomerController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create customer",
            Description = "Creates a new customer."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create( [FromBody] CreateCustomerCommand command,CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command,cancellationToken);

            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
           Summary = "Update customer",
           Description = "Updates an existing customer."
       )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id,[FromBody] UpdateCustomerCommand command,CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpDelete("{id}")]
        [SwaggerOperation(
    Summary = "Delete customer",
    Description = "Soft deletes an existing customer."
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
            var command = new DeleteCustomerCommand
            {
                Id = id
            };

            var result = await mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpGet("search")]
        [SwaggerOperation(
            Summary = "Search customers",
            Description = "Searches customers by name, mobile number, or email."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Search(
            [FromQuery] SearchCustomersQuery query,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                query,
                cancellationToken);

            return result.ToActionResult();
        }
    }
}