using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Models;
using Workspace_Management_System.Infrastructure.Persistence;
using Workspace_Management_System.Infrastructure.Persistence.Context;

namespace Workspace_Management_System.Infrastructure.Repositories
{
    public class CompanyRepository
        : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}