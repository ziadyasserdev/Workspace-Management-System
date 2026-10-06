
namespace Workspace_Management_System.Application.Features.Customers.Dtos;

public class CustomerDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public int? CompanyId { get; set; }
    public string CustomerType { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string Status { get; set; } = string.Empty;
}
