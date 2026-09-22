using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Companies.Commands.CreateCompany
{
    public class CreateCompanyCommand : IRequest<Result<int>>
    {
        public string Name { get; set; } = null!;
        public string ContactPerson { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Email { get; set; }
        public string? TaxNumber { get; set; }
        public string? TaxInformation { get; set; }
        public string? ContractDetails { get; set; }
        public int? PricingPlanId { get; set; }
        public decimal CreditLimit { get; set; }
    }
}