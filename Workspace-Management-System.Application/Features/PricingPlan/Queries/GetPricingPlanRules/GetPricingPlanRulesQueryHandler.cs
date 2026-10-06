using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.PricingPlan.Queries.GetPricingPlanRules;

public class GetPricingPlanRulesQueryHandler
    : IRequestHandler<
        GetPricingPlanRulesQuery,
        Result<List<PricingPlanRuleDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer _localizer;

    public GetPricingPlanRulesQueryHandler(
        IUnitOfWork unitOfWork,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Result<List<PricingPlanRuleDto>>> Handle(
        GetPricingPlanRulesQuery request,
        CancellationToken cancellationToken)
    {
        var pricingPlanExists = await _unitOfWork.PricingPlans
            .Query()
            .AnyAsync(
                x => x.Id == request.Id &&
                     !x.IsDeleted,
                cancellationToken);

        if (!pricingPlanExists)
        {
            return Result<List<PricingPlanRuleDto>>.Failure(
                ResultStatus.NotFound,
                _localizer["PricingPlanNotFound"]);
        }

        var rules = await _unitOfWork.PricingRules
            .Query()
            .AsNoTracking()
            .Where(x =>
                x.PricingPlanId == request.Id &&
                !x.IsDeleted)
            .OrderBy(x => x.WorkspaceTypeId)
            .ThenBy(x => x.RuleType)
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
            .ToListAsync(cancellationToken);

        var result = rules
            .Select(x => new PricingPlanRuleDto
            {
                Id = x.Id,
                PricingPlanId = x.PricingPlanId,
                WorkspaceTypeId = x.WorkspaceTypeId,
                RuleType = _localizer[
                    $"PricingRuleType_{x.RuleType}"],
                Value = x.Value,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                DayOfWeek = x.DayOfWeek.HasValue
                    ? _localizer[
                        $"DayOfWeek_{x.DayOfWeek.Value}"]
                    : null,
                IsActive = x.IsActive
            })
            .ToList();

        return Result<List<PricingPlanRuleDto>>.Success(result);
    }
}
