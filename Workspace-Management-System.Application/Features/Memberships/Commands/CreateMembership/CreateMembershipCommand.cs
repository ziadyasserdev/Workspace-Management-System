using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Memberships.Commands.CreateMembership
{
    public class CreateMembershipCommand : IRequest<Result<int>>
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public int DurationDays { get; set; }
        public decimal Price { get; set; }
    }
}
