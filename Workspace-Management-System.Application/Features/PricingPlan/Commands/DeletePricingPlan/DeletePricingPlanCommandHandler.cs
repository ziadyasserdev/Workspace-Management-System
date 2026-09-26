using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.PricingPlan.Commands.DeletePricingPlan
{
    public class DeletePricingPlanCommandHandler
        : IRequestHandler<DeletePricingPlanCommand, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUser;

        public DeletePricingPlanCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            this.unitOfWork = unitOfWork;
            this.currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            DeletePricingPlanCommand request,
            CancellationToken cancellationToken)
        {
            var pricingPlan = await unitOfWork.PricingPlans
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.IsDeleted,
                    cancellationToken);

            if (pricingPlan == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Pricing plan not found.");
            }

            var hasPricingRules = await unitOfWork.PricingRules
                .Query()
                .AnyAsync(
                    x => x.PricingPlanId == request.Id &&
                         !x.IsDeleted,
                    cancellationToken);

            if (hasPricingRules)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Pricing plan cannot be deleted because it has pricing rules.");
            }

            var isUsedByCompany = await unitOfWork.Companies
                .Query()
                .AnyAsync(
                    x => x.PricingPlanId == request.Id &&
                         !x.IsDeleted,
                    cancellationToken);

            if (isUsedByCompany)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Pricing plan cannot be deleted because it is assigned to a company.");
            }

      

            pricingPlan.IsDeleted = true;
            pricingPlan.IsDeletedBy = currentUser.UserId;
            pricingPlan.IsActive = false;
            pricingPlan.UpdatedAt = DateTime.UtcNow;
            pricingPlan.UpdatedBy = currentUser.UserId;

            await unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Pricing plan deleted successfully.");
        }
    }
}