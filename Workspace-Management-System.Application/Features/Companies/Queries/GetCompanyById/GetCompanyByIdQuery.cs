using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Companies.DTOs;

namespace Workspace_Management_System.Application.Features.Companies.Queries.GetCompanyById
{
    public class GetCompanyByIdQuery
        : IRequest<Result<CompanyDto>>
    {
        public int Id { get; set; }
    }
}