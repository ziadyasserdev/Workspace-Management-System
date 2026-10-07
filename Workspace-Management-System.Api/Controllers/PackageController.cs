using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Api.Attributes;
using Workspace_Management_System.Api.Common.Responses;
using Workspace_Management_System.Application.Features.Packages.Commands.ActivatePackage;
using Workspace_Management_System.Application.Features.Packages.Commands.AssignCustomer;
using Workspace_Management_System.Application.Features.Packages.Commands.CreatePackage;
using Workspace_Management_System.Application.Features.Packages.Commands.DeactivatePackage;
using Workspace_Management_System.Application.Features.Packages.Commands.DeletePackage;
using Workspace_Management_System.Application.Features.Packages.Commands.RemoveCustomer;
using Workspace_Management_System.Application.Features.Packages.Commands.RestorePackage;
using Workspace_Management_System.Application.Features.Packages.Commands.UpdatePackage;
using Workspace_Management_System.Application.Features.Packages.Commands.UpgradeCustomerPackage;
using Workspace_Management_System.Application.Features.Packages.Queries.GetPackageById;
using Workspace_Management_System.Application.Features.Packages.Queries.GetPackageCustomers;
using Workspace_Management_System.Application.Features.Packages.Queries.GetPackages;
using Workspace_Management_System.Domain.Constants;

namespace Workspace_Management_System.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PackageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PackageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [RequirePermission(Permissions.PackagesCreate)]
        [SwaggerOperation(
            Summary = "Create a new package",
            Description = "Creates a new package after validating package name, type, hours, duration, and price."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreatePackage(
            [FromBody] CreatePackageCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpGet]
        [RequirePermission(Permissions.PackagesView)]
        [SwaggerOperation(
            Summary = "Get packages",
            Description = "Returns packages with pagination and optional search."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPackages(
            [FromQuery] GetPackagesQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult();
        }

        [HttpGet("{id:int}")]
        [RequirePermission(Permissions.PackagesViewDetails)]
        [SwaggerOperation(
            Summary = "Get package by id",
            Description = "Returns detailed information about a package."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPackageById(
            int id,
            CancellationToken cancellationToken)
        {
            var query = new GetPackageByIdQuery { Id = id };
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult();
        }

        [HttpGet("{packageId:int}/customers")]
        [RequirePermission(Permissions.PackagesViewCustomers)]
        [SwaggerOperation(
            Summary = "Get package customers",
            Description = "Returns customers associated with a package with pagination and optional search."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPackageCustomers(
            int packageId,
            [FromQuery] string? search,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var query = new GetPackageCustomersQuery
            {
                PackageId = packageId,
                Search = search,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPut("{id:int}")]
        [RequirePermission(Permissions.PackagesUpdate)]
        [SwaggerOperation(
            Summary = "Update package",
            Description = "Updates an existing package after validating its package details."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdatePackage(
            int id,
            [FromBody] UpdatePackageCommand command,
            CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPatch("{id:int}/activate")]
        [RequirePermission(Permissions.PackagesActivate)]
        [SwaggerOperation(
            Summary = "Activate package",
            Description = "Activates an existing package."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ActivatePackage(
            int id,
            CancellationToken cancellationToken)
        {
            var command = new ActivatePackageCommand { Id = id };
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPatch("{id:int}/deactivate")]
        [RequirePermission(Permissions.PackagesDeactivate)]
        [SwaggerOperation(
            Summary = "Deactivate package",
            Description = "Deactivates an existing package."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeactivatePackage(
            int id,
            CancellationToken cancellationToken)
        {
            var command = new DeactivatePackageCommand { Id = id };
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPatch("{id:int}/restore")]
        [RequirePermission(Permissions.PackagesRestore)]
        [SwaggerOperation(
            Summary = "Restore package",
            Description = "Restores a previously soft-deleted package."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> RestorePackage(
            int id,
            CancellationToken cancellationToken)
        {
            var command = new RestorePackageCommand { Id = id };
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpDelete("{id:int}")]
        [RequirePermission(Permissions.PackagesDelete)]
        [SwaggerOperation(
            Summary = "Delete package",
            Description = "Soft deletes a package after validating that it has no active customers."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeletePackage(
            int id,
            CancellationToken cancellationToken)
        {
            var command = new DeletePackageCommand { Id = id };
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPost("{packageId:int}/customers")]
        [RequirePermission(Permissions.PackagesAssignCustomer)]
        [SwaggerOperation(
            Summary = "Assign customer to package",
            Description = "Assigns a customer to an active package."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AssignCustomer(
            int packageId,
            [FromBody] AssignCustomerCommand command,
            CancellationToken cancellationToken)
        {
            command.PackageId = packageId;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpDelete("{packageId:int}/customers/{customerId:int}")]
        [RequirePermission(Permissions.PackagesRemoveCustomer)]
        [SwaggerOperation(
            Summary = "Remove customer from package",
            Description = "Removes an active customer package relationship."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveCustomer(
            int packageId,
            int customerId,
            CancellationToken cancellationToken)
        {
            var command = new RemoveCustomerCommand
            {
                PackageId = packageId,
                CustomerId = customerId
            };
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPut("customers/{customerId:int}/upgrade")]
        [RequirePermission(Permissions.PackagesUpgradeCustomer)]
        [SwaggerOperation(
            Summary = "Upgrade customer package",
            Description = "Upgrades a customer's current package to another active package."
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpgradeCustomerPackage(
            int customerId,
            [FromBody] UpgradeCustomerPackageCommand command,
            CancellationToken cancellationToken)
        {
            command.CustomerId = customerId;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult();
        }
    }
}