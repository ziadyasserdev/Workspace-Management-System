using Workspace_Management_System.Domain.Models;

public class SessionProduct : BaseEntity
{
    public int SessionId { get; set; }
    public int ProductId { get; set; }

    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public Session Session { get; set; } = null!;
    public Product Product { get; set; } = null!;
}