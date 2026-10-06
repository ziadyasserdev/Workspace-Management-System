using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.PricingRule.Commands.RestorePricingRule
{
    public class RestorePricingRuleCommandHandler
        : IRequestHandler<RestorePricingRuleCommand, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUser;
        private readonly IStringLocalizer _localizer;

        public RestorePricingRuleCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            IStringLocalizerFactory factory)
        {
            this.unitOfWork = unitOfWork;
            this.currentUser = currentUser;
            _localizer = factory.Create(typeof(SharedResources));
        }

        public async Task<Result<bool>> Handle(
            RestorePricingRuleCommand request,
            CancellationToken cancellationToken)
        {
            var pricingRule = await unitOfWork.PricingRules
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && x.IsDeleted,
                    cancellationToken);

            if (pricingRule == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["DeletedPricingRuleNotFound"]);
            }

            pricingRule.IsDeleted = false;
            pricingRule.IsDeletedBy = null;
            pricingRule.IsActive = true;
            pricingRule.UpdatedAt = DateTime.UtcNow;
            pricingRule.UpdatedBy = currentUser.UserId;

            await unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                _localizer["PricingRuleRestoredSuccessfully"]);
        }
    }
}