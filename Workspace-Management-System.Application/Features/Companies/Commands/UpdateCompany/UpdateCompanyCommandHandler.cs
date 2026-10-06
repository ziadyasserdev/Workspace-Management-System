using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.Companies.Commands.UpdateCompany
{
    public class UpdateCompanyCommandHandler
        : IRequestHandler<UpdateCompanyCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UpdateCompanyCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
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
                    "Company not found.");
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
                    "A company with the same name already exists.");
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
                        "Pricing plan not found.");
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
                "Company updated successfully.");
        }
    }
}