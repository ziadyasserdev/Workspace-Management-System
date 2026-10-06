using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.PricingRule.Queries;
using Workspace_Management_System.Application.Features.PricingRules.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.PricingRule.Queries.GetPricingRuleById
{
    public class GetPricingRuleByIdQueryHandler
        : IRequestHandler<GetPricingRuleByIdQuery, Result<PricingRuleDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IStringLocalizer _localizer;

        public GetPricingRuleByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IStringLocalizerFactory factory)
        {
            this.unitOfWork = unitOfWork;
            _localizer = factory.Create(typeof(SharedResources));
        }

        public async Task<Result<PricingRuleDto>> Handle(
            GetPricingRuleByIdQuery request,
            CancellationToken cancellationToken)
        {
            var pricingRule = await unitOfWork.PricingRules
                .Query()
                .AsNoTracking()
                .Where(x =>
                    x.Id == request.Id &&
                    !x.IsDeleted)
                .Select(x => new
                {
                    x.Id,
                    x.PricingPlanId,
                    x.WorkspaceTypeId,
                    x.RuleType,
                    x.Value,
                    x.StartDate,
                    x.EndDate,
                    x.DayOfWeek,
                    x.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (pricingRule == null)
            {
                return Result<PricingRuleDto>.Failure(
                    ResultStatus.NotFound,
                    _localizer["PricingRuleNotFound"]);
            }

            var result = new PricingRuleDto
            {
                Id = pricingRule.Id,
                PricingPlanId = pricingRule.PricingPlanId,
                WorkspaceTypeId = pricingRule.WorkspaceTypeId,
                RuleType = _localizer[
                    $"PricingRuleType_{pricingRule.RuleType}"],
                Value = pricingRule.Value,
                StartDate = pricingRule.StartDate,
                EndDate = pricingRule.EndDate,
                DayOfWeek = pricingRule.DayOfWeek.HasValue
                    ? _localizer[
                        $"DayOfWeek_{pricingRule.DayOfWeek.Value}"]
                    : null,
                IsActive = pricingRule.IsActive
            };

            return Result<PricingRuleDto>.Success(
                result,
                _localizer["PricingRuleRetrievedSuccessfully"]);
        }
    }
}

