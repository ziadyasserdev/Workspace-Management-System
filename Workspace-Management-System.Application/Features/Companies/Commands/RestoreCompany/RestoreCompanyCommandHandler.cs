using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.Companies.Commands.RestoreCompany
{
    public class RestoreCompanyCommandHandler
        : IRequestHandler<RestoreCompanyCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public RestoreCompanyCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            RestoreCompanyCommand request,
            CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.Companies
                .Query()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id,
                    cancellationToken);

            if (company is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Company not found.");
            }

            if (!company.IsDeleted)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Company is already active.");
            }

            var activeCompanyWithSameName = await _unitOfWork.Companies
                .Query()
                .AnyAsync(
                    x =>
                        x.Id != request.Id &&
                        (x.NameEn == company.NameEn ||
                         x.NameAr == company.NameAr) &&
                        !x.IsDeleted,
                    cancellationToken);

            if (activeCompanyWithSameName)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Another active company with the same name already exists.");
            }

            company.IsDeleted = false;
            company.IsDeletedBy = null;
            company.UpdatedAt = DateTime.UtcNow;
            company.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Company restored successfully.");
        }
    }
}