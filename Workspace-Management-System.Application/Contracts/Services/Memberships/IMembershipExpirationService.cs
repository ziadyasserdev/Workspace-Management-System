using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Contracts.Services.Memberships
{
    public interface IMembershipExpirationService
    {
        Task ExpireMembershipsAsync(
            CancellationToken cancellationToken = default);
    }
}
