using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Discounts.Dtos
{
    public class DiscountResponseDto
    {
        public int Id { get; set; }

        public string NameEn { get; set; } = string.Empty;

        public string NameAr { get; set; } = string.Empty;

        public string? DescriptionEn { get; set; }

        public string? DescriptionAr { get; set; }

        public DiscountType Type { get; set; }

        public decimal Value { get; set; }

        public bool IsActive { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}