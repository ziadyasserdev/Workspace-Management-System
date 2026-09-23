using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Infrastructure.Persistence.Context;

namespace Workspace_Management_System.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;



        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Workspaces = new WorkspaceRepository(_context);
            WorkspaceTypes = new WorkspaceTypeRepository(_context);
            Customers = new CustomerRepository(_context);
            Companies = new CompanyRepository(_context);
            Employees = new EmployeeRepository(_context);
        }

       
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

     
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

       
        private bool _disposed = false;

        public IWorkspaceRepository Workspaces { get; private set; } = null!;

        public IWorkspaceTypeRepository WorkspaceTypes { get; private set; } = null!;
        public ICustomerRepository Customers { get; private set; } = null!;
        public ICompanyRepository Companies { get; private set; } = null!;

        public IEmployeeRepository Employees { get;private set; } = null!;  

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
