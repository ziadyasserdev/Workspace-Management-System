using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Packages.Commands.UpgradeCustomerPackage;

public class UpgradeCustomerPackageCommand : IRequest<Result<int>>
{
    public int CustomerId { get; set; }

    public int NewPackageId { get; set; }
}