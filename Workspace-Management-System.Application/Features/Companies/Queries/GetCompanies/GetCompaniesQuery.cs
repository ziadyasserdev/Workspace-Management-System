using MediatR;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Companies.DTOs;

namespace Workspace_Management_System.Application.Features.Companies.Queries.GetCompanies
{
    public class GetCompaniesQuery
        : IRequest<Result<PaginatedResult<CompanyDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}