using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Contracts.Services.Memberships;

namespace Workspace_Management_System.Infrastructure.BackgroundJobs
{
    public class MembershipExpirationJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MembershipExpirationJob(
            IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ExecuteAsync()
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var expirationService =
                scope.ServiceProvider
                    .GetRequiredService<IMembershipExpirationService>();

            await expirationService.ExpireMembershipsAsync();
        }
    }
}
