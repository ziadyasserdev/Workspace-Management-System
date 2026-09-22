using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Companies.Commands.DeleteCompany
{
    public class DeleteCompanyCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}