using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Models;
using Workspace_Management_System.Infrastructure.Persistence.Context;

namespace Workspace_Management_System.Infrastructure.Repositories
{
    public class SessionProductRepository
        : GenericRepository<SessionProduct>,
          ISessionProductRepository
    {
        public SessionProductRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}