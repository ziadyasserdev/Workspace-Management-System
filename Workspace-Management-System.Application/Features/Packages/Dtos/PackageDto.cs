using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Packages.Dtos;

public class PackageDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public PackageType PackageType { get; set; }

    public decimal TotalHours { get; set; }

    public int? DurationDays { get; set; }

    public decimal Price { get; set; }

    public bool IsActive { get; set; }

    public int CustomerCount { get; set; }

    public List<PackageCustomerDto> Customers { get; set; }
        = new();
}