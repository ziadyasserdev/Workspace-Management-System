using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Companies.DTOs;

namespace Workspace_Management_System.Application.Features.Companies.Queries.GetCompanyById
{
    public class GetCompanyByIdQueryHandler
        : IRequestHandler<
            GetCompanyByIdQuery,
            Result<CompanyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompanyByIdQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                    "Company not found.");
            }

            var result = new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                ContactPerson = company.ContactPerson,
                Phone = company.Phone,
                Email = company.Email,
                TaxNumber = company.TaxNumber,
                TaxInformation = company.TaxInformation,
                ContractDetails = company.ContractDetails,
                PricingPlanId = company.PricingPlanId,
                CreditLimit = company.CreditLimit,
                IsActive = company.IsActive
            };

            return Result<CompanyDto>.Success(
                result,
                "Company retrieved successfully.");
        }
    }
}