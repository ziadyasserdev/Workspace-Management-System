using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Models;

public class Customer : BaseEntity
{
    public string FullNameEn { get; set; } = null!;
    public string FullNameAr { get; set; } = null!;

    public string MobileNumber { get; set; } = null!;
    public string? Email { get; set; }

    public int? CompanyId { get; set; }

    public CustomerType CustomerType { get; set; }

    public string? NotesEn { get; set; }
    public string? NotesAr { get; set; }

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