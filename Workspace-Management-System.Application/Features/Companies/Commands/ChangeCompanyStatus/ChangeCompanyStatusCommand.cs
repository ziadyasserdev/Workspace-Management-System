using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Companies.Commands.ChangeCompanyStatus
{
    public class ChangeCompanyStatusCommand
        : IRequest<Result<bool>>
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
    }
}