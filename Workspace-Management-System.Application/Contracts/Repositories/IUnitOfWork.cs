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
        IWorkspaceRepository Workspaces { get; }
        IWorkspaceTypeRepository WorkspaceTypes { get; }
        ICustomerRepository Customers { get; }
        IEmployeeRepository Employees { get; }
        Task<int> SaveAsync();
    }

}
