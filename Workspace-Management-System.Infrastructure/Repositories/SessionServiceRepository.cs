using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Models;
using Workspace_Management_System.Infrastructure.Persistence.Context;

namespace Workspace_Management_System.Infrastructure.Repositories;

public class SessionServiceRepository
    : GenericRepository<SessionService>, ISessionServiceRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SessionServiceRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SessionService?> GetBySessionAndServiceAsync(
        int sessionId,
        int serviceId)
    {
        return await _dbContext.SessionServices
            .FirstOrDefaultAsync(x =>
                x.SessionId == sessionId &&
                x.ServiceId == serviceId);
    }
}
