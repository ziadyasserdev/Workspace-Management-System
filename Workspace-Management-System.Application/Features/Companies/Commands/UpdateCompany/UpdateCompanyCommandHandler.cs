using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Companies.Commands.UpdateCompany
{
    public class UpdateCompanyCommandHandler
        : IRequestHandler<UpdateCompanyCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer _localizer;

        public UpdateCompanyCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            IStringLocalizerFactory factory)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _localizer = factory.Create(typeof(SharedResources));
        }

        public async Task<Result<bool>> Handle(
            UpdateCompanyCommand request,
            CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Companies
                .Query()
                .FirstOrDefaultAsync(
                    x =>
                        x.Id == request.Id &&
                        !x.IsDeleted,
                    cancellationToken);

            if (company is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["CompanyNotFound"]);
            }

            var exists = await _unitOfWork.Companies
                .Query()
                .AnyAsync(
                    x =>
                        x.Id != request.Id &&
                        (x.NameEn == request.NameEn ||
                         x.NameAr == request.NameAr) &&
                        !x.IsDeleted,
                    cancellationToken);

            if (exists)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["CompanySameNameAlreadyExists"]);
            }

            if (request.PricingPlanId.HasValue)
            {
                var pricingPlanExists = await _unitOfWork.PricingPlans
                    .Query()
                    .AnyAsync(
                        x =>
                            x.Id == request.PricingPlanId.Value &&
                            !x.IsDeleted,
                        cancellationToken);

                if (!pricingPlanExists)
                {
                    return Result<bool>.Failure(
                        ResultStatus.NotFound,
                        _localizer["PricingPlanNotFound"]);
                }
            }

            company.NameEn = request.NameEn.Trim();
            company.NameAr = request.NameAr.Trim();
            company.ContactPerson = request.ContactPerson;
            company.Phone = request.Phone;
            company.Email = request.Email;
            company.TaxNumber = request.TaxNumber;
            company.TaxInformationEn = request.TaxInformationEn?.Trim();
            company.TaxInformationAr = request.TaxInformationAr?.Trim();
            company.ContractDetailsEn = request.ContractDetailsEn?.Trim();
            company.ContractDetailsAr = request.ContractDetailsAr?.Trim();
            company.PricingPlanId = request.PricingPlanId;
            company.CreditLimit = request.CreditLimit;
            company.UpdatedAt = DateTime.UtcNow;
            company.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                _localizer["CompanyUpdatedSuccessfully"]);
        }
    }
}
