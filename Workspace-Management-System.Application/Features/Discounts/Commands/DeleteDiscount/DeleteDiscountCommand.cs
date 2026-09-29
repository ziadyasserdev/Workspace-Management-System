using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Discounts.Commands.DeleteDiscount
{
    public class DeleteDiscountCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}