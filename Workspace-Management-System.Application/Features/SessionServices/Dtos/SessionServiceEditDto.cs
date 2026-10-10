
namespace Workspace_Management_System.Application.Features.Sessions.Services.Dtos;

public class SessionServiceEditDto
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public int ServiceId { get; set; }
    public string ServiceNameEn { get; set; } = string.Empty;
    public string ServiceNameAr { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}
