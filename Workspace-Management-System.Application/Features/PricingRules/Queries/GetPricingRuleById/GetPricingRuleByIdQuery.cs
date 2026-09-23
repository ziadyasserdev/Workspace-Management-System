using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.PricingRules.Dtos;

namespace Workspace_Management_System.Application.Features.PricingRule.Queries.GetPricingRuleById
{
    public class GetPricingRuleByIdQuery
        : IRequest<Result<PricingRuleDto>>
    {
        public int Id { get; set; }
    }
}