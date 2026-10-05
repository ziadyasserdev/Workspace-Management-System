using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Contracts.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
       
        Task<IDbContextTransaction> BeginTransactionAsync();
        ICompanyRepository Companies { get; }
        IMembershipRepository Memberships { get; }
        ICustomerMembershipRepository CustomerMemberships { get; }
        IMembershipBenefitRepository MembershipBenefits { get; }
        IWorkspaceRepository Workspaces { get; }
        IWorkspaceTypeRepository WorkspaceTypes { get; }
        ICustomerRepository Customers { get; }
        IEmployeeRepository Employees { get; }
        IBookingRepository Bookings { get; }
        ISessionWorkspaceHistoryRepository SessionWorkspaceHistories { get; }
        ISessionRepository Sessions { get; }
        IPricingPlanRepository PricingPlans { get; }
        IPricingRuleRepository PricingRules { get; }
        ITransactionRepository Transactions { get; }
        IProductCategoryRepository ProductCategories { get; }
        IProductRepository Products { get; }
        ISessionProductRepository SessionProducts { get; }
        IDiscountRepository Discounts { get; }  
        IServiceRepository Services { get; }
        ISessionServiceRepository SessionServices { get; }
        IPackageRepository Packages { get; }
        ICustomerPackageRepository CustomerPackages { get; }
        Task<int> SaveAsync();
    }

}
