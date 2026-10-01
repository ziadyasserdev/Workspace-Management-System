using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Api.Common.Responses;
using Workspace_Management_System.Application.Features.CustomerMemberships.Commands.CancelCustomerMembership;
using Workspace_Management_System.Application.Features.CustomerMemberships.Commands.RenewCustomerMembership;
using Workspace_Management_System.Application.Features.CustomerMemberships.Commands.ResumeCustomerMembership;
using Workspace_Management_System.Application.Features.CustomerMemberships.Commands.SubscribeCustomer;
using Workspace_Management_System.Application.Features.CustomerMemberships.Commands.SuspendCustomerMembership;
using Workspace_Management_System.Application.Features.CustomerMemberships.Queries.GetCurrentCustomerMembership;
using Workspace_Management_System.Application.Features.CustomerMemberships.Queries.GetCustomerMemberships;

namespace Workspace_Management_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerMembershipController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerMembershipController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        [HttpPost("{customerId:int}/memberships")]
        [SwaggerOperation(
    Summary = "Subscribe customer to a membership",
    Description = "Creates a new membership subscription for a customer."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SubscribeCustomer(
    int customerId,
    [FromBody] SubscribeCustomerCommand command,
    CancellationToken cancellationToken)
        {
            command.CustomerId = customerId;

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return result.ToActionResult();
        }
        [HttpGet("{customerId:int}/memberships")]
        [SwaggerOperation(
    Summary = "Get customer memberships",
    Description = "Returns all memberships of a customer."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomerMemberships(
    int customerId,
    CancellationToken cancellationToken)
        {
            var query = new GetCustomerMembershipsQuery
            {
                CustomerId = customerId
            };


            var result = await _mediator.Send(
                query,
                cancellationToken);


            return result.ToActionResult();
        }
        [HttpGet("{customerId:int}/memberships/current")]
        [SwaggerOperation(
    Summary = "Get current customer membership",
    Description = "Returns the currently active membership of a customer."
)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCurrentMembership(
    int customerId,
    CancellationToken cancellationToken)
        {

            var query = new GetCurrentCustomerMembershipQuery
            {
                CustomerId = customerId
            };


            var result =
                await _mediator.Send(
                    query,
                    cancellationToken);


            return result.ToActionResult();
        }
        [ApiController]
        [Route("api/[controller]")]
        public class CustomerMembershipsController : ControllerBase
        {
            private readonly IMediator _mediator;

            public CustomerMembershipsController(IMediator mediator)
            {
                _mediator = mediator;
            }

            [HttpPatch("{id:int}/cancel")]
            [SwaggerOperation(
                Summary = "Cancel customer membership",
                Description = "Cancels an existing customer membership."
            )]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status409Conflict)]
            public async Task<IActionResult> Cancel(
                int id,
                CancellationToken cancellationToken)
            {
                var command = new CancelCustomerMembershipCommand
                {
                    Id = id
                };

                var result = await _mediator.Send(
                    command,
                    cancellationToken);

                return result.ToActionResult();
            }
            [HttpPatch("{id:int}/suspend")]
            [SwaggerOperation(
    Summary = "Suspend customer membership",
    Description = "Suspends an active customer membership."
)]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status409Conflict)]
            public async Task<IActionResult> Suspend(
    int id,
    CancellationToken cancellationToken)
            {
                var command = new SuspendCustomerMembershipCommand
                {
                    Id = id
                };

                var result = await _mediator.Send(
                    command,
                    cancellationToken);

                return result.ToActionResult();
            }
            [HttpPatch("{id:int}/resume")]
            [SwaggerOperation(
    Summary = "Resume customer membership",
    Description = "Resumes a suspended customer membership."
)]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status409Conflict)]
            public async Task<IActionResult> Resume(
    int id,
    CancellationToken cancellationToken)
            {

                var command = new ResumeCustomerMembershipCommand
                {
                    Id = id
                };


                var result =
                    await _mediator.Send(
                        command,
                        cancellationToken);


                return result.ToActionResult();
            }
            [HttpPost("{id:int}/renew")]
            [SwaggerOperation(
    Summary = "Renew customer membership",
    Description = "Creates a new membership subscription based on the previous membership plan."
)]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status409Conflict)]
            public async Task<IActionResult> Renew(
    int id,
    CancellationToken cancellationToken)
            {
                var command = new RenewCustomerMembershipCommand
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
}
