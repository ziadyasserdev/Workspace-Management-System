using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.PricingRule.Commands.DeletePricingRule
{
    public class DeletePricingRuleCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}