using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Discounts.Dtos;

namespace Workspace_Management_System.Application.Features.Discounts.Queries.GetDiscountById
{
    public class GetDiscountByIdQuery
        : IRequest<Result<DiscountEditDto>>
    {
        public int Id { get; set; }
    }
}