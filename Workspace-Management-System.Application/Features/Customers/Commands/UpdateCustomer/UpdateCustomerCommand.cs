using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }

        public string FullNameEn { get; set; } = null!;

        public string FullNameAr { get; set; } = null!;

        public string MobileNumber { get; set; } = null!;

        public string? Email { get; set; }

        public int? CompanyId { get; set; }

        public CustomerType CustomerType { get; set; }

        public string? NotesEn { get; set; }

        public string? NotesAr { get; set; }
    }
}