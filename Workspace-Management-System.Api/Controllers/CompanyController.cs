using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Workspace_Management_System.Api.Attributes;
using Workspace_Management_System.Api.Common.Responses;
using Workspace_Management_System.Application.Features.Companies.Commands.ChangeCompanyStatus;
using Workspace_Management_System.Application.Features.Companies.Commands.CreateCompany;
using Workspace_Management_System.Application.Features.Companies.Commands.DeleteCompany;
using Workspace_Management_System.Application.Features.Companies.Commands.RestoreCompany;
using Workspace_Management_System.Application.Features.Companies.Commands.UpdateCompany;
using Workspace_Management_System.Application.Features.Companies.Queries.GetCompanies;
using Workspace_Management_System.Application.Features.Companies.Queries.GetCompanyById;
using Workspace_Management_System.Application.Features.Companies.Queries.GetCompanyCustomers;
using Workspace_Management_System.Application.Features.Companies.Queries.SearchCompanies;
using Workspace_Management_System.Domain.Constants;

namespace Workspace_Management_System.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CompanyController : ControllerBase
{
    private readonly IMediator mediator;

    public CompanyController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    [RequirePermission(Permissions.CompaniesCreate)]
    [SwaggerOperation(
        Summary = "Create company",
        Description = "Creates a new company."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCompanyCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            command,
            cancellationToken);

        return result.ToActionResult();
    }

    [HttpPut("{id}")]
    [RequirePermission(Permissions.CompaniesUpdate)]
    [SwaggerOperation(
        Summary = "Update company",
        Description = "Updates an existing company."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateCompanyCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;

        var result = await mediator.Send(
            command,
            cancellationToken);

        return result.ToActionResult();
    }

    [HttpDelete("{id}")]
    [RequirePermission(Permissions.CompaniesDelete)]
    [SwaggerOperation(
        Summary = "Delete company",
        Description = "Soft deletes an existing company."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCompanyCommand
        {
            Id = id
        };

        var result = await mediator.Send(
            command,
            cancellationToken);

        return result.ToActionResult();
    }

    [HttpGet]
    [RequirePermission(Permissions.CompaniesView)]
    [SwaggerOperation(
        Summary = "Get companies",
        Description = "Retrieves a paginated list of companies."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCompanies(
        [FromQuery] GetCompaniesQuery query,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            query,
            cancellationToken);

        return result.ToActionResult();
    }

    [HttpGet("{id}")]
    [RequirePermission(Permissions.CompaniesViewDetails)]
    [SwaggerOperation(
        Summary = "Get company by ID",
        Description = "Retrieves a company by ID."
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
        var query = new GetCompanyByIdQuery
        {
            Id = id
        };

        var result = await mediator.Send(
            query,
            cancellationToken);

        return result.ToActionResult();
    }

    [HttpPatch("{id}/status")]
    [RequirePermission(Permissions.CompaniesChangeStatus)]
    [SwaggerOperation(
        Summary = "Change company status",
        Description = "Activates or deactivates an existing company."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ChangeStatus(
        int id,
        [FromBody] ChangeCompanyStatusCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;

        var result = await mediator.Send(
            command,
            cancellationToken);

        return result.ToActionResult();
    }

    [HttpGet("search")]
    [RequirePermission(Permissions.CompaniesSearch)]
    [SwaggerOperation(
        Summary = "Search companies",
        Description = "Searches companies by name, contact person, phone, or email."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Search(
        [FromQuery] SearchCompaniesQuery query,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            query,
            cancellationToken);

        return result.ToActionResult();
    }

    [HttpGet("{id}/customers")]
    [RequirePermission(Permissions.CompaniesViewCustomers)]
    [SwaggerOperation(
        Summary = "Get company customers",
        Description = "Retrieves customers belonging to a company."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomers(
        int id,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCompanyCustomersQuery
        {
            CompanyId = id,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await mediator.Send(
            query,
            cancellationToken);

        return result.ToActionResult();
    }

    [HttpPatch("{id}/restore")]
    [RequirePermission(Permissions.CompaniesRestore)]
    [SwaggerOperation(
        Summary = "Restore company",
        Description = "Restores a soft deleted company."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Restore(
        int id,
        CancellationToken cancellationToken)
    {
        var command = new RestoreCompanyCommand
        {
            Id = id
        };

        var result = await mediator.Send(
            command,
            cancellationToken);

        return result.ToActionResult();
    }
}
