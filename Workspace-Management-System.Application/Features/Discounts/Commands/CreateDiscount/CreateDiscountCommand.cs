using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Discounts.Commands.CreateDiscount
{
    public class CreateDiscountCommand : IRequest<Result<int>>
    {
        public string Name { get; set; } = null!;

        public Domain.Enums.DiscountType Type { get; set; }

        public decimal Value { get; set; }

        public bool IsActive { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}