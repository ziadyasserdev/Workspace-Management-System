using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.PricingPlan.Commands.RestorePricingPlan
{
    public class RestorePricingPlanCommandHandler
        : IRequestHandler<RestorePricingPlanCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer _localizer;

        public RestorePricingPlanCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            IStringLocalizerFactory factory)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _localizer = factory.Create(typeof(SharedResources));
        }

        public async Task<Result<bool>> Handle(
            RestorePricingPlanCommand request,
            CancellationToken cancellationToken)
        {
            var pricingPlan = await _unitOfWork.PricingPlans
                .Query()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && x.IsDeleted,
                    cancellationToken);

            if (pricingPlan == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["DeletedPricingPlanNotFound"]);
            }

            var duplicateName = await _unitOfWork.PricingPlans
                .Query()
                .IgnoreQueryFilters()
                .AnyAsync(
                    x =>
                        x.Id != request.Id &&
                        !x.IsDeleted &&
                        (
                            x.NameEn.ToLower() == pricingPlan.NameEn.ToLower() ||
                            x.NameAr.ToLower() == pricingPlan.NameAr.ToLower()
                        ),
                    cancellationToken);

            if (duplicateName)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["PricingPlanWithSameNameAlreadyExists"]);
            }

            pricingPlan.IsDeleted = false;
            pricingPlan.IsDeletedBy = null;
            pricingPlan.IsActive = true;
            pricingPlan.UpdatedAt = DateTime.UtcNow;
            pricingPlan.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                _localizer["PricingPlanRestoredSuccessfully"]);
        }
    }
}