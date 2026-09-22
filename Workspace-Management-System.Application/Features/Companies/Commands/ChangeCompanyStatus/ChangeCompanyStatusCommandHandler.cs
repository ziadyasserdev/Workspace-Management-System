using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.Companies.Commands.ChangeCompanyStatus
{
    public class ChangeCompanyStatusCommandHandler
        : IRequestHandler<
            ChangeCompanyStatusCommand,
            Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChangeCompanyStatusCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(
            ChangeCompanyStatusCommand request,
            CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Companies
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id,
                    cancellationToken);

            if (company is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Company not found.");
            }

            if (company.IsDeleted)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Company is deleted.");
            }

            company.IsActive = request.IsActive;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Company status changed successfully.");
        }
    }
}