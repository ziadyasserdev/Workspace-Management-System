using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Identity;
using Workspace_Management_System.Domain.Models;
using static Azure.Core.HttpHeader;
using static System.Collections.Specialized.BitVector32;

namespace Workspace_Management_System.Infrastructure.Persistence.Context
{
    //public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    //{
    //    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    //    {

    //    }

    //    protected override void OnModelCreating(ModelBuilder builder)
    //    {
    //        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    //        base.OnModelCreating(builder);
    //    }


    //}
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

     

        public DbSet<Company> Companies => Set<Company>();
        public DbSet<Customer> Customers => Set<Customer>();

        public DbSet<WorkspaceType> WorkspaceTypes => Set<WorkspaceType>();
        public DbSet<Workspace> Workspaces => Set<Workspace>();

        public DbSet<PricingPlan> PricingPlans => Set<PricingPlan>();
        public DbSet<PricingRule> PricingRules => Set<PricingRule>();

        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Session> Sessions => Set<Session>();
        public DbSet<SessionWorkspaceHistory> SessionWorkspaceHistories
            => Set<SessionWorkspaceHistory>();

        public DbSet<ProductCategory> ProductCategories
            => Set<ProductCategory>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Inventory> Inventories => Set<Inventory>();
        public DbSet<StockMovement> StockMovements => Set<StockMovement>();

        public DbSet<Service> Services => Set<Service>();

        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<TransactionItem> TransactionItems
            => Set<TransactionItem>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Invoice> Invoices => Set<Invoice>();

        public DbSet<Discount> Discounts => Set<Discount>();

        public DbSet<Package> Packages => Set<Package>();
        public DbSet<CustomerPackage> CustomerPackages
            => Set<CustomerPackage>();

        public DbSet<Membership> Memberships => Set<Membership>();
        public DbSet<MembershipBenefit> MembershipBenefits
            => Set<MembershipBenefit>();
        public DbSet<CustomerMembership> CustomerMemberships
            => Set<CustomerMembership>();

        public DbSet<ExpenseCategory> ExpenseCategories
            => Set<ExpenseCategory>();
        public DbSet<Expense> Expenses => Set<Expense>();

        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Employee> Employees => Set<Employee>();

        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<Setting> Settings => Set<Setting>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomerMembership>()
                .HasKey(cm => new
                {
                    cm.CustomerId,
                    cm.MembershipId
                });
            modelBuilder.Entity<SessionWorkspaceHistory>()
    .HasOne(x => x.Session)
    .WithMany(x => x.WorkspaceHistory)
    .HasForeignKey(x => x.SessionId)
    .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<CustomerMembership>()
                .HasOne(cm => cm.Customer)
                .WithMany(c => c.CustomerMemberships)
                .HasForeignKey(cm => cm.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Transaction>()
    .HasOne(x => x.Session)
    .WithMany(x => x.Transactions)
    .HasForeignKey(x => x.SessionId)
    .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Invoice>()
    .HasOne(x => x.Transaction)
    .WithOne(x => x.Invoice)
    .HasForeignKey<Invoice>(x => x.TransactionId)
    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CustomerMembership>()
                .HasOne(cm => cm.Membership)
                .WithMany(m => m.CustomerMemberships)
                .HasForeignKey(cm => cm.MembershipId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Sender)
                .WithMany()
                .HasForeignKey(n => n.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Receiver)
                .WithMany()
                .HasForeignKey(n => n.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
