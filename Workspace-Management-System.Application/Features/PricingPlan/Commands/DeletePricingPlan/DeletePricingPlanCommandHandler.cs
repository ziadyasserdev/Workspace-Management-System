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

            pricingPlan.IsDeleted = true;
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