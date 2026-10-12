using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommand : IRequest<Result<bool>>
    {
        public int EmployeeId { get; set; }

        public string FirstNameEn { get; set; } = string.Empty;
        public string FirstNameAr { get; set; } = string.Empty;

        public string LastNameEn { get; set; } = string.Empty;
        public string LastNameAr { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
