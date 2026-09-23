using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.PricingPlan.Commands.UpdatePricingPlan
{
    public class UpdatePricingPlanCommandHandler
        : IRequestHandler<UpdatePricingPlanCommand, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUser;

        public UpdatePricingPlanCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            this.unitOfWork = unitOfWork;
            this.currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            UpdatePricingPlanCommand request,
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

            var duplicateName = await unitOfWork.PricingPlans
                .Query()
                .AnyAsync(
                    x => x.Id != request.Id &&
                         x.Name == request.Name &&
                         !x.IsDeleted,
                    cancellationToken);

            if (duplicateName)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "A pricing plan with the same name already exists.");
            }

            pricingPlan.Name = request.Name;
            pricingPlan.Description = request.Description;
            pricingPlan.IsActive = request.IsActive;
            pricingPlan.UpdatedAt = DateTime.UtcNow;
            pricingPlan.UpdatedBy = currentUser.UserId;

            await unitOfWork.SaveAsync();
            return Result<bool>.Success(
                true,
                "Pricing plan updated successfully.");
        }
    }
}