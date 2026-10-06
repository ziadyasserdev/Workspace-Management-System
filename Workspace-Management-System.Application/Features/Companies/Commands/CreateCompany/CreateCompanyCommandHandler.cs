
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Companies.Commands.CreateCompany
{
    public class CreateCompanyCommandHandler
        : IRequestHandler<CreateCompanyCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer _localizer;

        public CreateCompanyCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            IStringLocalizerFactory factory)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _localizer = factory.Create(typeof(SharedResources));
        }

        public async Task<Result<int>> Handle(
            CreateCompanyCommand request,
            CancellationToken cancellationToken)
        {
            var exists = await _unitOfWork.Companies
                .Query()
                .AnyAsync(
                    x =>
                        (x.NameEn == request.NameEn ||
                         x.NameAr == request.NameAr) &&
                        !x.IsDeleted,
                    cancellationToken);

            if (exists)
            {
                return Result<int>.Failure(
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
                    return Result<int>.Failure(
                        ResultStatus.NotFound,
                        _localizer["PricingPlanNotFound"]);
                }
            }

            var company = new Company
            {
                NameEn = request.NameEn.Trim(),
                NameAr = request.NameAr.Trim(),
                ContactPerson = request.ContactPerson,
                Phone = request.Phone,
                Email = request.Email,
                TaxNumber = request.TaxNumber,
                TaxInformationEn = request.TaxInformationEn?.Trim(),
                TaxInformationAr = request.TaxInformationAr?.Trim(),
                ContractDetailsEn = request.ContractDetailsEn?.Trim(),
                ContractDetailsAr = request.ContractDetailsAr?.Trim(),
                PricingPlanId = request.PricingPlanId,
                CreditLimit = request.CreditLimit,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _currentUser.UserId
            };

            await _unitOfWork.Companies.AddAsync(company);
            await _unitOfWork.SaveAsync();

            return Result<int>.Success(
                company.Id,
                _localizer["CompanyCreatedSuccessfully"]);
        }
    }
}
