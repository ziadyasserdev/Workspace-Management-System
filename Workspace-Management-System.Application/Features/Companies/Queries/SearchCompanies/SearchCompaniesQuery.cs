using MediatR;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Companies.DTOs;

namespace Workspace_Management_System.Application.Features.Companies.Queries.SearchCompanies
{
    public class SearchCompaniesQuery
        : IRequest<Result<PaginatedResult<CompanyDto>>>
    {
        public string SearchTerm { get; set; } = null!;

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}