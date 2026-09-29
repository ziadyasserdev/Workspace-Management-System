using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Models;
using Workspace_Management_System.Infrastructure.Persistence.Context;

namespace Workspace_Management_System.Infrastructure.Repositories
{
    public class DiscountRepository
        : GenericRepository<Discount>,
          IDiscountRepository
    {
        public DiscountRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}