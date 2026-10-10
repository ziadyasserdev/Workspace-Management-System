
namespace Workspace_Management_System.Application.Features.Customers.Dtos;

public class CustomerEditDto
{
    public int Id { get; set; }
    public string FullNameEn { get; set; } = string.Empty;
    public string FullNameAr { get; set; } = string.Empty;
    public string? MobileNumber { get; set; }
    public string? Email { get; set; }
    public int? CompanyId { get; set; }
    public string CustomerType { get; set; } = string.Empty;
    public string? NotesEn { get; set; }
    public string? NotesAr { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string Status { get; set; } = string.Empty;
}
