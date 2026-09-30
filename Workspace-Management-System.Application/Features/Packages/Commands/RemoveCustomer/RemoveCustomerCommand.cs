using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Packages.Commands.RemoveCustomer;

public class RemoveCustomerCommand : IRequest<Result<int>>
{
    public int PackageId { get; set; }

    public int CustomerId { get; set; }
}