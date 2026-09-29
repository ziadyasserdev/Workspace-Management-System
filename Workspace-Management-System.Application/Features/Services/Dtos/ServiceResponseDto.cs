namespace Workspace_Management_System.Application.Features.Services.Dtos
{
    public class ServiceResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}

