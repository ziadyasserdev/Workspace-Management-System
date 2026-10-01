using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Contracts.Services.Memberships
{
    public class MembershipExpirationService : IMembershipExpirationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MembershipExpirationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExpireMembershipsAsync(
            CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            var memberships = await _unitOfWork.CustomerMemberships
                .Query()
                .Where(x =>
                    !x.IsDeleted &&
                    x.Status == CustomerMembershipStatus.Active &&
                    x.EndDate <= now)
                .ToListAsync(cancellationToken);

            foreach (var membership in memberships)
            {
                membership.Status = CustomerMembershipStatus.Expired;
                membership.UpdatedAt = now;
                membership.UpdatedBy = "System";
            }

            if (memberships.Count > 0)
            {
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
