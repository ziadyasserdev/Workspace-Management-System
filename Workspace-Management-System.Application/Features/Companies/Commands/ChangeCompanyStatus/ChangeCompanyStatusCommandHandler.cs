using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Companies.Commands.ChangeCompanyStatus
{
    public class ChangeCompanyStatusCommandHandler
        : IRequestHandler<
            ChangeCompanyStatusCommand,
            Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer _localizer;

        public ChangeCompanyStatusCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            IStringLocalizerFactory factory)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _localizer = factory.Create(typeof(SharedResources));
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
                    _localizer["CompanyNotFound"]);
            }

            if (company.IsDeleted)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["CompanyIsDeleted"]);
            }

            company.IsActive = request.IsActive;

            company.UpdatedAt = DateTime.UtcNow;
            company.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                _localizer["CompanyStatusChangedSuccessfully"]);
        }
    }
}
