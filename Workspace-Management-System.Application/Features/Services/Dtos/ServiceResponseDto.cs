namespace Workspace_Management_System.Application.Features.Services.Dtos
{
    public class ServiceResponseDto
    {
        public int Id { get; set; }

        public string NameEn { get; set; } = null!;
        public string NameAr { get; set; } = null!;

        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }

        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}