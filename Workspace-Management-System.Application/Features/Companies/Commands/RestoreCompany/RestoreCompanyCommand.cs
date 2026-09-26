using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Companies.Commands.RestoreCompany
{
    public class RestoreCompanyCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}