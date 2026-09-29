using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Discounts.Dtos;

namespace Workspace_Management_System.Application.Features.Discounts.Queries.GetDiscountById
{
    public class GetDiscountByIdQuery
        : IRequest<Result<DiscountResponseDto>>
    {
        public int Id { get; set; }
    }
}