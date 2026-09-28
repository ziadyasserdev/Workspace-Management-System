using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Models;
using Workspace_Management_System.Infrastructure.Persistence.Context;

namespace Workspace_Management_System.Infrastructure.Repositories
{
    public class TransactionRepository: GenericRepository<Transaction>,ITransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TransactionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
