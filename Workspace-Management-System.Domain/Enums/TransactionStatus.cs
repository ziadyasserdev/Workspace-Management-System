using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Domain.Enums
{
    public enum TransactionStatus
    {
        Pending,
        Completed,
        Cancelled,
        Refunded,
        PartiallyRefunded
    }
}
