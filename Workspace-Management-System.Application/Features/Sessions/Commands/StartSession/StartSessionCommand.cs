using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Sessions.Commands.StartSession
{
    public class StartSessionCommand : IRequest<Result<int>>
    {
        public int CustomerId { get; set; }

        public int? BookingId { get; set; }

        public int WorkspaceId { get; set; }

        public int PricingPlanId { get; set; }

        public int NumberOfPeople { get; set; }
    }
}
