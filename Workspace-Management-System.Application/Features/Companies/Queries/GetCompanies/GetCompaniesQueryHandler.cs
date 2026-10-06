using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Companies.DTOs;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Companies.Queries.GetCompanies;

public class GetCompaniesQueryHandler
    : IRequestHandler<
        GetCompaniesQuery,
        Result<PaginatedResult<CompanyDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IStringLocalizer _localizer;

    public GetCompaniesQueryHandler(
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Result<PaginatedResult<CompanyDto>>> Handle(
        GetCompaniesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Companies
            .Query()
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        var totalCount = await query.CountAsync(
            cancellationToken);

        var companies = await query
            .OrderBy(x => x.NameEn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var items = companies
            .Select(x => new CompanyDto
            {
                Id = x.Id,
                Name = _localizationService.GetLocalizedValue(
                    x.NameEn,
                    x.NameAr),
                ContactPerson = x.ContactPerson,
                Phone = x.Phone,
                Email = x.Email,
                TaxNumber = x.TaxNumber,
                TaxInformation = _localizationService.GetLocalizedValue(
                    x.TaxInformationEn,
                    x.TaxInformationAr),
                ContractDetails = _localizationService.GetLocalizedValue(
                    x.ContractDetailsEn,
                    x.ContractDetailsAr),
                PricingPlanId = x.PricingPlanId,
                CreditLimit = x.CreditLimit,
                IsActive = x.IsActive
            })
            .ToList();

        var result = new PaginatedResult<CompanyDto>(
            items,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result<PaginatedResult<CompanyDto>>.Success(
            result,
            _localizer["CompaniesRetrievedSuccessfully"]);
    }
}
