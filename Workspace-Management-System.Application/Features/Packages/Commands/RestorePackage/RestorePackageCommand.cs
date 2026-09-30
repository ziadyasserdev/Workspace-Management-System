using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Packages.Commands.RestorePackage;

public class RestorePackageCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
}