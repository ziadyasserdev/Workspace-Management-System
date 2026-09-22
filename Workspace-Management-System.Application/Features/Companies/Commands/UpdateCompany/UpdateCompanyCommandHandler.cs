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
                    x => x.Id == request.Id &&
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
                    x => x.Id != request.Id &&
                         x.Name == request.Name &&
                         !x.IsDeleted,
                    cancellationToken);

            if (exists)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "A company with the same name already exists.");
            }

            company.Name = request.Name;
            company.ContactPerson = request.ContactPerson;
            company.Phone = request.Phone;
            company.Email = request.Email;
            company.TaxNumber = request.TaxNumber;
            company.TaxInformation = request.TaxInformation;
            company.ContractDetails = request.ContractDetails;
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