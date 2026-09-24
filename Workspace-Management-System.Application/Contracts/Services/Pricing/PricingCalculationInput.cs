namespace Workspace_Management_System.Application.Services.Pricing;

public class PricingCalculationInput
{
    public DateTime StartTime { get; init; }

    public DateTime EndTime { get; init; }

    public decimal HourlyRate { get; init; }

    public decimal? HalfHourRate { get; init; }

    public decimal MinimumCharge { get; init; }

    public decimal? FullDayMaximum { get; init; }

    public int RoundingMinutes { get; init; }

    public bool RoundUp { get; init; }
}