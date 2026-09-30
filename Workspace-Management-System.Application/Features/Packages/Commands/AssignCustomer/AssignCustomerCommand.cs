using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Packages.Commands.AssignCustomer;

public class AssignCustomerCommand : IRequest<Result<int>>
{
    public int PackageId { get; set; }

    public int CustomerId { get; set; }
}