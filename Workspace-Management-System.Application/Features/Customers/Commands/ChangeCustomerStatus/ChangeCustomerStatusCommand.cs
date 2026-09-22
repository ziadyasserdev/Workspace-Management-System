using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Customers.Commands.ChangeCustomerStatus
{
    public class ChangeCustomerStatusCommand
        : IRequest<Result<bool>>
    {
        public int Id { get; set; }
        public CustomerStatus Status { get; set; }
    }
}