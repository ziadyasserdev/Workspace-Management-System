using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Packages.Dtos;

namespace Workspace_Management_System.Application.Features.Packages.Queries.GetPackageById;

public class GetPackageByIdQuery
    : IRequest<Result<PackageEditDto>>
{
    public int Id { get; set; }
}