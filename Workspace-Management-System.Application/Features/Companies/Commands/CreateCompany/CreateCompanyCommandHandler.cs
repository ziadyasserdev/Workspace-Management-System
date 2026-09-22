using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Companies.Commands.CreateCompany
{
    public class CreateCompanyCommandHandler
        : IRequestHandler<CreateCompanyCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public CreateCompanyCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<int>> Handle(
            CreateCompanyCommand request,
            CancellationToken cancellationToken)
        {
            var exists = await _unitOfWork.Companies
                .Query()
                .AnyAsync(
                    x => x.Name == request.Name &&
                         !x.IsDeleted,
                    cancellationToken);

            if (exists)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "A company with the same name already exists.");
            }

            var company = new Company
            {
                Name = request.Name,
                ContactPerson = request.ContactPerson,
                Phone = request.Phone,
                Email = request.Email,
                TaxNumber = request.TaxNumber,
                TaxInformation = request.TaxInformation,
                ContractDetails = request.ContractDetails,
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
                "Company created successfully.");
        }
    }
}