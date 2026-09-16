using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Contracts.Identity
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? Email { get; }
        string? UserName { get; }
        bool IsAuthenticated { get; }
        //bool IsDeleted { get; }
        IEnumerable<string> Roles { get; }

        public bool IsInRole(string role);

    }
}
