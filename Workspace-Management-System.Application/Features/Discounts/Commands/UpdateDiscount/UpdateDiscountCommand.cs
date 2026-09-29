using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Discounts.Commands.UpdateDiscount
{
    public class UpdateDiscountCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public DiscountType Type { get; set; }

        public decimal Value { get; set; }

        public bool IsActive { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}