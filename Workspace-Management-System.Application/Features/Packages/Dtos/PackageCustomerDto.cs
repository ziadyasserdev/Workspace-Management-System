using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Packages.Dtos;

public class PackageCustomerDto
{
    public int CustomerId { get; set; }
    public string FullName { get; set; } = null!;
    public string MobileNumber { get; set; } = null!;
    public CustomerPackageStatus Status { get; set; }

    public DateTime PurchaseDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public decimal? RemainingHours { get; set; }
}