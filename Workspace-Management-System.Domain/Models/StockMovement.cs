using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Domain.Models
{
    public class StockMovement : BaseEntity
    {
        public int ProductId { get; set; }

        public string MovementType { get; set; } = null!;
        public int Quantity { get; set; }

        public int QuantityBefore { get; set; }
        public int QuantityAfter { get; set; }

        public int UserId { get; set; }

        public string? Reason { get; set; }
        public string ReferenceType { get; set; } = null!;
        public int? ReferenceId { get; set; }

        public Product Product { get; set; } = null!;
    }
}
