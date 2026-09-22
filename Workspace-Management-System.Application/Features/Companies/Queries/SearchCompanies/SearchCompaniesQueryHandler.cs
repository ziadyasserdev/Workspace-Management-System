using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Companies.DTOs;

namespace Workspace_Management_System.Application.Features.Companies.Queries.SearchCompanies
{
    public class SearchCompaniesQueryHandler
        : IRequestHandler<
            SearchCompaniesQuery,
            Result<PaginatedResult<CompanyDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchCompaniesQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<CompanyDto>>> Handle(
            SearchCompaniesQuery request,
            CancellationToken cancellationToken)
        {
            var searchTerm = request.SearchTerm.Trim();

            var query = _unitOfWork.Companies
                .Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .Where(x =>
                    x.Name.Contains(searchTerm) ||
                    x.ContactPerson.Contains(searchTerm) ||
                    x.Phone.Contains(searchTerm) ||
                    (x.Email != null &&
                     x.Email.Contains(searchTerm)));

            var totalCount = await query.CountAsync(
                cancellationToken);

            var items = await query
                .OrderBy(x => x.Name)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CompanyDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    ContactPerson = x.ContactPerson,
                    Phone = x.Phone,
                    Email = x.Email,
                    TaxNumber = x.TaxNumber,
                    TaxInformation = x.TaxInformation,
                    ContractDetails = x.ContractDetails,
                    PricingPlanId = x.PricingPlanId,
                    CreditLimit = x.CreditLimit,
                    IsActive = x.IsActive
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<CompanyDto>(
                items,
                request.PageNumber,
                request.PageSize,
                totalCount);

            return Result<PaginatedResult<CompanyDto>>.Success(
                result,
                "Companies retrieved successfully.");
        }
    }
}