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
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUser;

        public RestorePricingPlanCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            this.unitOfWork = unitOfWork;
            this.currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            RestorePricingPlanCommand request,
            CancellationToken cancellationToken)
        {
            var pricingPlan = await unitOfWork.PricingPlans
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && x.IsDeleted,
                    cancellationToken);

            if (pricingPlan == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Deleted pricing plan not found.");
            }

            var duplicateName = await unitOfWork.PricingPlans
                .Query()
                .AnyAsync(
                    x => x.Id != request.Id &&
                         x.Name == pricingPlan.Name &&
                         !x.IsDeleted,
                    cancellationToken);

            if (duplicateName)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "A pricing plan with the same name already exists.");
            }

            pricingPlan.IsDeleted = false;
            pricingPlan.IsActive = true;
            pricingPlan.UpdatedAt = DateTime.UtcNow;
            pricingPlan.UpdatedBy = currentUser.UserId;

            await unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Pricing plan restored successfully.");
        }
    }
}