using Workspace_Management_System.Application.Contracts.Pricing;

namespace Workspace_Management_System.Application.Services.Pricing;

public class PricingCalculator : IPricingCalculator
{
    public PricingResult Calculate(PricingCalculationInput input)
    {
        if (input.EndTime < input.StartTime)
        {
            throw new ArgumentException("End time cannot be before start time.");
        }

        var duration = input.EndTime - input.StartTime;

        var billableDuration = ApplyRounding(
            duration,
            input.RoundingMinutes,
            input.RoundUp);

        var totalMinutes = billableDuration.TotalMinutes;

        var baseAmount =
            (decimal)totalMinutes / 60m * input.HourlyRate;

        var minimumChargeApplied = 0m;

        if (baseAmount < input.MinimumCharge)
        {
            minimumChargeApplied = input.MinimumCharge - baseAmount;
            baseAmount = input.MinimumCharge;
        }

        var fullDayMaximumApplied = 0m;

        if (input.FullDayMaximum.HasValue &&
            baseAmount > input.FullDayMaximum.Value)
        {
            fullDayMaximumApplied =
                baseAmount - input.FullDayMaximum.Value;

            baseAmount = input.FullDayMaximum.Value;
        }

        return new PricingResult
        {
            Duration = duration,
            BillableDuration = billableDuration,
            BaseAmount = baseAmount,
            MinimumChargeApplied = minimumChargeApplied,
            FullDayMaximumApplied = fullDayMaximumApplied,
            FinalAmount = baseAmount
        };
    }

    private static TimeSpan ApplyRounding(
        TimeSpan duration,
        int roundingMinutes,
        bool roundUp)
    {
        if (roundingMinutes <= 0)
        {
            return duration;
        }

        var totalMinutes = duration.TotalMinutes;

        var roundedMinutes = roundUp
            ? Math.Ceiling(totalMinutes / roundingMinutes) * roundingMinutes
            : Math.Floor(totalMinutes / roundingMinutes) * roundingMinutes;

        return TimeSpan.FromMinutes(roundedMinutes);
    }
}