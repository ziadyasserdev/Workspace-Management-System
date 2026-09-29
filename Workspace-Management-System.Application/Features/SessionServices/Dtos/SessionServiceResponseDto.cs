
namespace Workspace_Management_System.Application.Features.Sessions.Services.Dtos;

public class SessionServiceResponseDto
{
    public int Id { get; set; }

    public int SessionId { get; set; }

    public int ServiceId { get; set; }

    public string ServiceName { get; set; } = null!;

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }
}
