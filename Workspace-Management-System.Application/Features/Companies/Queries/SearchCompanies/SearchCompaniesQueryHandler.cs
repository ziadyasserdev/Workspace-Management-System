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
                    x.NameEn.Contains(searchTerm) ||
                    x.NameAr.Contains(searchTerm) ||
                    x.ContactPerson.Contains(searchTerm) ||
                    x.Phone.Contains(searchTerm) ||
                    (x.Email != null &&
                     x.Email.Contains(searchTerm)) ||
                    (x.TaxNumber != null &&
                     x.TaxNumber.Contains(searchTerm)) ||
                    (x.TaxInformationEn != null &&
                     x.TaxInformationEn.Contains(searchTerm)) ||
                    (x.TaxInformationAr != null &&
                     x.TaxInformationAr.Contains(searchTerm)) ||
                    (x.ContractDetailsEn != null &&
                     x.ContractDetailsEn.Contains(searchTerm)) ||
                    (x.ContractDetailsAr != null &&
                     x.ContractDetailsAr.Contains(searchTerm)));

            var totalCount = await query.CountAsync(
                cancellationToken);

            var items = await query
                .OrderBy(x => x.NameEn)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CompanyDto
                {
                    Id = x.Id,
                    NameEn = x.NameEn,
                    NameAr = x.NameAr,
                    ContactPerson = x.ContactPerson,
                    Phone = x.Phone,
                    Email = x.Email,
                    TaxNumber = x.TaxNumber,
                    TaxInformationEn = x.TaxInformationEn,
                    TaxInformationAr = x.TaxInformationAr,
                    ContractDetailsEn = x.ContractDetailsEn,
                    ContractDetailsAr = x.ContractDetailsAr,
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