using Microsoft.EntityFrameworkCore.Storage;

namespace Workspace_Management_System.Application.Contracts.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task<IDbContextTransaction> BeginTransactionAsync();

        ICompanyRepository Companies { get; }
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

        Task<int> SaveAsync();
    }
}
