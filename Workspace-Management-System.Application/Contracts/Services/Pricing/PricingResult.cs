namespace Workspace_Management_System.Application.Services.Pricing;

public class PricingResult
{
    public TimeSpan Duration { get; init; }

    public TimeSpan BillableDuration { get; init; }

    public decimal BaseAmount { get; init; }

    public decimal MinimumChargeApplied { get; init; }

    public decimal FullDayMaximumApplied { get; init; }

    public decimal FinalAmount { get; init; }
}