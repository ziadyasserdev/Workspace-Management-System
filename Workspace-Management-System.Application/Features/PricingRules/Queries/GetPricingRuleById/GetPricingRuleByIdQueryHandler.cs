using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.PricingRule.Queries;
using Workspace_Management_System.Application.Features.PricingRules.Dtos;

namespace Workspace_Management_System.Application.Features.PricingRule.Queries.GetPricingRuleById
{
    public class GetPricingRuleByIdQueryHandler
        : IRequestHandler<GetPricingRuleByIdQuery, Result<PricingRuleDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetPricingRuleByIdQueryHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<PricingRuleDto>> Handle(
            GetPricingRuleByIdQuery request,
            CancellationToken cancellationToken)
        {
            var pricingRule = await unitOfWork.PricingRules
                .Query()
                .Where(x =>
                    x.Id == request.Id &&
                    !x.IsDeleted)
                .Select(x => new PricingRuleDto
                {
                    Id = x.Id,
                    PricingPlanId = x.PricingPlanId,
                    WorkspaceTypeId = x.WorkspaceTypeId,
                    RuleType = x.RuleType,
                    Value = x.Value,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    DayOfWeek = x.DayOfWeek,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (pricingRule == null)
            {
                return Result<PricingRuleDto>.Failure(
                    ResultStatus.NotFound,
                    "Pricing rule not found.");
            }

            return Result<PricingRuleDto>.Success(
                pricingRule);
        }
    }
}