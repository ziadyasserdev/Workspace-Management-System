using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Customers.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string MobileNumber { get; set; }

        public string? Email { get; set; }

        public int? CompanyId { get; set; }

        public string CustomerType { get; set; }

        public string? Notes { get; set; }

        public DateTime RegistrationDate { get; set; }

        public CustomerStatus Status { get; set; }
    }
}
