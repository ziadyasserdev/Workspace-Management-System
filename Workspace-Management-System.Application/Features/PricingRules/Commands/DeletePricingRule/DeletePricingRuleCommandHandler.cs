using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.PricingRule.Commands.DeletePricingRule
{
    public class DeletePricingRuleCommandHandler
        : IRequestHandler<DeletePricingRuleCommand, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUser;

        public DeletePricingRuleCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            this.unitOfWork = unitOfWork;
            this.currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            DeletePricingRuleCommand request,
            CancellationToken cancellationToken)
        {
            var pricingRule = await unitOfWork.PricingRules
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.IsDeleted,
                    cancellationToken);

            if (pricingRule == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Pricing rule not found.");
            }

            pricingRule.IsDeleted = true;
            pricingRule.IsActive = false;
            pricingRule.UpdatedAt = DateTime.UtcNow;
            pricingRule.UpdatedBy = currentUser.UserId;

            await unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Pricing rule deleted successfully.");
        }
    }
}