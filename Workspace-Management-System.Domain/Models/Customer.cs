using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;
using static System.Collections.Specialized.BitVector32;

namespace Workspace_Management_System.Domain.Models
{
    public class Customer : BaseEntity
    {
        public string FullName { get; set; } = null!;
        public string MobileNumber { get; set; } = null!;
        public string? Email { get; set; }

        public int? CompanyId { get; set; }

        public string CustomerType { get; set; } = null!;
        public string? Notes { get; set; }
        public DateTime RegistrationDate { get; set; }
        public CustomerStatus Status { get; set; } 

        public Company? Company { get; set; }

        public ICollection<Booking> Bookings { get; set; }
            = new List<Booking>();

        public ICollection<Session> Sessions { get; set; }
            = new List<Session>();

        public ICollection<Transaction> Transactions { get; set; }
            = new List<Transaction>();

        public ICollection<Invoice> Invoices { get; set; }
            = new List<Invoice>();

        public ICollection<CustomerPackage> CustomerPackages { get; set; }
            = new List<CustomerPackage>();

        public ICollection<CustomerMembership> CustomerMemberships { get; set; }
            = new List<CustomerMembership>();
    }
}
