namespace Workspace_Management_System.Application.Features.SessionProducts.Dtos
{
    public class SessionProductResponseDto
    {
        public int Id { get; set; }

        public int SessionId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total { get; set; }
    }
}