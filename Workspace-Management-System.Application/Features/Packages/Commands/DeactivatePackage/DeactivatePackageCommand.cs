using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Packages.Commands.DeactivatePackage;

public class DeactivatePackageCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
}