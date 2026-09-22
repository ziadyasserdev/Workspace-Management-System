using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.Companies.Commands.DeleteCompany
{
    public class DeleteCompanyCommandHandler
        : IRequestHandler<DeleteCompanyCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeleteCompanyCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            DeleteCompanyCommand request,
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
                    "Company is already deleted.");
            }

            company.IsDeleted = true;
            company.UpdatedAt = DateTime.UtcNow;
            company.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Company deleted successfully.");
        }
    }
}