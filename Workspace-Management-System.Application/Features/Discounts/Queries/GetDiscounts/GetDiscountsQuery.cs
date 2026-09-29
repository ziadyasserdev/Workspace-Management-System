using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Discounts.Dtos;

namespace Workspace_Management_System.Application.Features.Discounts.Queries.GetDiscounts
{
    public class GetDiscountsQuery
        : IRequest<Result<List<DiscountResponseDto>>>
    {
        public bool? IsActive { get; set; }
    }
}