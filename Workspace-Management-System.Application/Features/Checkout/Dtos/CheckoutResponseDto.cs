namespace Workspace_Management_System.Application.Features.Checkout.Dtos;

public class CheckoutResponseDto
{
    public int TransactionId { get; set; }

    public string TransactionNumber { get; set; } = string.Empty;

    public int SessionId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public TimeSpan Duration { get; set; }

    public decimal WorkspaceAmount { get; set; }

    public decimal ProductsAmount { get; set; }

    public decimal ServicesAmount { get; set; }

    public decimal Subtotal { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal Total { get; set; }

    public string Status { get; set; } = string.Empty;

    public List<CheckoutResponseItemDto> Items { get; set; } = new();
}
public class CheckoutResponseItemDto
{
    public string Description { get; set; } = string.Empty;

    public string ItemType { get; set; } = string.Empty;

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Total { get; set; }
}