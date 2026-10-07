using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Api.Attributes;
using Workspace_Management_System.Api.Common.Responses;
using Workspace_Management_System.Application.Features.Discounts.Commands.CreateDiscount;
using Workspace_Management_System.Application.Features.Discounts.Commands.DeleteDiscount;
using Workspace_Management_System.Application.Features.Discounts.Commands.RestoreDiscount;
using Workspace_Management_System.Application.Features.Discounts.Commands.UpdateDiscount;
using Workspace_Management_System.Application.Features.Discounts.Queries.GetDiscountById;
using Workspace_Management_System.Application.Features.Discounts.Queries.GetDiscounts;
using Workspace_Management_System.Domain.Constants;

namespace Workspace_Management_System.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DiscountController : ControllerBase
{
    private readonly IMediator _mediator;

    public DiscountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [RequirePermission(Permissions.DiscountsCreate)]
    [SwaggerOperation(
        Summary = "Create discount",
        Description = "Creates a new discount."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateDiscountCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command,
            cancellationToken);

        return result.ToActionResult();
    }

    [HttpPut("{id:int}")]
    [RequirePermission(Permissions.DiscountsUpdate)]
    [SwaggerOperation(
        Summary = "Update discount",
        Description = "Updates an existing discount."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateDiscountCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;

        var result = await _mediator.Send(
            command,
            cancellationToken);

        return result.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    [RequirePermission(Permissions.DiscountsDelete)]
    [SwaggerOperation(
        Summary = "Delete discount",
        Description = "Soft deletes an existing discount."
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
        var command = new DeleteDiscountCommand
        {
            Id = id
        };

        var result = await _mediator.Send(
            command,
            cancellationToken);

        return result.ToActionResult();
    }

    [HttpPatch("{id:int}/restore")]
    [RequirePermission(Permissions.DiscountsRestore)]
    [SwaggerOperation(
        Summary = "Restore discount",
        Description = "Restores a previously soft-deleted discount."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Restore(
        int id,
        CancellationToken cancellationToken)
    {
        var command = new RestoreDiscountCommand
        {
            Id = id
        };

        var result = await _mediator.Send(
            command,
            cancellationToken);

        return result.ToActionResult();
    }

    [HttpGet]
    [RequirePermission(Permissions.DiscountsView)]
    [SwaggerOperation(
        Summary = "Get discounts",
        Description = "Returns all non-deleted discounts with optional search, active status filter, and pagination."
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
        var query = new GetDiscountsQuery
        {
            Search = search,
            IsActive = isActive,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(
            query,
            cancellationToken);

        return result.ToActionResult();
    }

    [HttpGet("{id:int}")]
    [RequirePermission(Permissions.DiscountsViewDetails)]
    [SwaggerOperation(
        Summary = "Get discount by ID",
        Description = "Retrieves a discount by ID."
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
        var query = new GetDiscountByIdQuery
        {
            Id = id
        };

        var result = await _mediator.Send(
            query,
            cancellationToken);

        return result.ToActionResult();
    }
}
