using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Packages.Commands.DeletePackage;

public class DeletePackageCommand : IRequest<Result<int>>
{
    public int Id { get; set; }
}