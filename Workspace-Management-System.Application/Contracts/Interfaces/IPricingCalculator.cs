using Workspace_Management_System.Application.Services.Pricing;

namespace Workspace_Management_System.Application.Contracts.Pricing;

public interface IPricingCalculator
{
    PricingResult Calculate(PricingCalculationInput input);
}