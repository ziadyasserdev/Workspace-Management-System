namespace Workspace_Management_System.Application.Features.Checkout.Dtos;

public class CheckoutRequestDto
{
    public int? DiscountId { get; set; }

    public decimal? TaxRate { get; set; }
}