using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.PricingPlan.Commands.RestorePricingPlan
{
    public class RestorePricingPlanCommandHandler
        : IRequestHandler<RestorePricingPlanCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public RestorePricingPlanCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
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
                    "Deleted pricing plan not found.");
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
                    "A pricing plan with the same name already exists.");
            }

            pricingPlan.IsDeleted = false;
            pricingPlan.IsDeletedBy = null;
            pricingPlan.IsActive = true;
            pricingPlan.UpdatedAt = DateTime.UtcNow;
            pricingPlan.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Pricing plan restored successfully.");
        }
    }
}