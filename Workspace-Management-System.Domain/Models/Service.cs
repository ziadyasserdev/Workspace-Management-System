using System.Collections.Generic;

namespace Workspace_Management_System.Domain.Models
{
    public class Service : BaseEntity
    {
        public string NameEn { get; set; } = null!;
        public string NameAr { get; set; } = null!;

        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }

        public decimal Price { get; set; }
        public bool IsActive { get; set; }

        public ICollection<SessionService> SessionServices { get; set; }
            = new List<SessionService>();

        public ICollection<TransactionItem> TransactionItems { get; set; }
            = new List<TransactionItem>();
    }
}