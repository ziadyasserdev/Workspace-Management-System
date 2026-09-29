using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Models;
using Workspace_Management_System.Infrastructure.Persistence.Context;

namespace Workspace_Management_System.Infrastructure.Repositories
{
<<<<<<< Updated upstream
    public class SessionRepository:
     GenericRepository<Session>, ISessionRepository
    {
        public SessionRepository(ApplicationDbContext dbContext)
            : base(dbContext)
=======
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
>>>>>>> Stashed changes
    {
        public SessionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
}
