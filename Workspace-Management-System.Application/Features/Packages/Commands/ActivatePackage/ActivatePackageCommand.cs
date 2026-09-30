using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Packages.Commands.ActivatePackage;

public class ActivatePackageCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
}