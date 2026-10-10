using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Companies.DTOs;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Companies.Queries.GetCompanyById;

public class GetCompanyByIdQueryHandler
    : IRequestHandler<GetCompanyByIdQuery, Result<CompanyEditDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer _localizer;

    public GetCompanyByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Result<CompanyEditDto>> Handle(
        GetCompanyByIdQuery request,
        CancellationToken cancellationToken)
    {
        var company = await _unitOfWork.Companies
            .Query()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken);

        if (company is null)
        {
            return Result<CompanyEditDto>.Failure(
                ResultStatus.NotFound,
                _localizer["CompanyNotFound"]);
        }

        var result = new CompanyEditDto
        {
            Id = company.Id,
            NameEn = company.NameEn,
            NameAr = company.NameAr,
            ContactPerson = company.ContactPerson,
            Phone = company.Phone,
            Email = company.Email,
            TaxNumber = company.TaxNumber,
            TaxInformationEn = company.TaxInformationEn,
            TaxInformationAr = company.TaxInformationAr,
            ContractDetailsEn = company.ContractDetailsEn,
            ContractDetailsAr = company.ContractDetailsAr,
            PricingPlanId = company.PricingPlanId,
            CreditLimit = company.CreditLimit,
            IsActive = company.IsActive
        };

        return Result<CompanyEditDto>.Success(
            result,
            _localizer["CompanyRetrievedSuccessfully"]);
    }
}
