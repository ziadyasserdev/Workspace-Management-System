using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Employees.Commands.ChangeEmployeeStatus
{
    public class ChangeEmployeeStatusCommand : IRequest<Result<bool>>
    {
        public int EmployeeId { get; set; }

        public EmployeeStatus Status { get; set; }
    }
}
