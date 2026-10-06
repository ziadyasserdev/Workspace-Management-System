using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Companies.DTOs;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Companies.Queries.GetCompanyById;

public class GetCompanyByIdQueryHandler
    : IRequestHandler<
        GetCompanyByIdQuery,
        Result<CompanyDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IStringLocalizer _localizer;

    public GetCompanyByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Result<CompanyDto>> Handle(
        GetCompanyByIdQuery request,
        CancellationToken cancellationToken)
    {
        var company = await _unitOfWork.Companies
            .Query()
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (company is null)
        {
            return Result<CompanyDto>.Failure(
                ResultStatus.NotFound,
                _localizer["CompanyNotFound"]);
        }

        var result = new CompanyDto
        {
            Id = company.Id,
            Name = _localizationService.GetLocalizedValue(
                company.NameEn,
                company.NameAr),
            ContactPerson = company.ContactPerson,
            Phone = company.Phone,
            Email = company.Email,
            TaxNumber = company.TaxNumber,
            TaxInformation = _localizationService.GetLocalizedValue(
                company.TaxInformationEn,
                company.TaxInformationAr),
            ContractDetails = _localizationService.GetLocalizedValue(
                company.ContractDetailsEn,
                company.ContractDetailsAr),
            PricingPlanId = company.PricingPlanId,
            CreditLimit = company.CreditLimit,
            IsActive = company.IsActive
        };

        return Result<CompanyDto>.Success(
            result,
            _localizer["CompanyRetrievedSuccessfully"]);
    }
}
