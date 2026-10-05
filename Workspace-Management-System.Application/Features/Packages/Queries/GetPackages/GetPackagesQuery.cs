using MediatR;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Packages.Dtos;

namespace Workspace_Management_System.Application.Features.Packages.Queries.GetPackages;

public class GetPackagesQuery
    : IRequest<Result<PaginatedResult<PackageDto>>>
{
    public string? Search { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}