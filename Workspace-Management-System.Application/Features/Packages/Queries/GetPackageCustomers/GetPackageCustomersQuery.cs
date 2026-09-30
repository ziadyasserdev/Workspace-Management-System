using MediatR;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Packages.Dtos;

namespace Workspace_Management_System.Application.Features.Packages.Queries.GetPackageCustomers;

public class GetPackageCustomersQuery
    : IRequest<Result<PaginatedResult<PackageCustomerDto>>>
{
    public int PackageId { get; set; }

    public string? Search { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}