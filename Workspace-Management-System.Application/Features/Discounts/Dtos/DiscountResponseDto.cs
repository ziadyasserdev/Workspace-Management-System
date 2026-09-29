using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Discounts.Dtos
{
    public class DiscountResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DiscountType Type { get; set; }

        public decimal Value { get; set; }

        public bool IsActive { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}