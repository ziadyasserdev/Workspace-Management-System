using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Domain.Models
{
    public class Inventory : BaseEntity
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public int MinimumStockLevel { get; set; }
        public int? MaximumStockLevel { get; set; }

        public Product Product { get; set; } = null!;
    }
}
