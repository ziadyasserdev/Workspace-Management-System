using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Discounts.Commands.RestoreDiscount
{
    public class RestoreDiscountCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}