namespace Workspace_Management_System.Application.Features.SessionProducts.Dtos;

public class SessionProductEditDto
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public int ProductId { get; set; }
    public string ProductNameEn { get; set; } = string.Empty;
    public string ProductNameAr { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total { get; set; }
}