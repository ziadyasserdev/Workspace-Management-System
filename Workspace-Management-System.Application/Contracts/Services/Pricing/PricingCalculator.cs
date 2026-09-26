using Workspace_Management_System.Application.Contracts.Pricing;

namespace Workspace_Management_System.Application.Services.Pricing;

public class PricingCalculator : IPricingCalculator
{
    public PricingResult Calculate(PricingCalculationInput input)
    {
        if (input.EndTime < input.StartTime)
        {
            throw new ArgumentException(
                "End time cannot be before start time.");
        }

        if (input.HourlyRate < 0)
        {
            throw new ArgumentException(
                "Hourly rate cannot be negative.");
        }

        if (input.HalfHourRate.HasValue &&
            input.HalfHourRate.Value < 0)
        {
            throw new ArgumentException(
                "Half-hour rate cannot be negative.");
        }

        if (input.MinimumCharge < 0)
        {
            throw new ArgumentException(
                "Minimum charge cannot be negative.");
        }

        if (input.FullDayMaximum.HasValue &&
            input.FullDayMaximum.Value < 0)
        {
            throw new ArgumentException(
                "Full-day maximum cannot be negative.");
        }

        var duration = input.EndTime - input.StartTime;

        var billableDuration = ApplyRounding(
            duration,
            input.RoundingMinutes,
            input.RoundingMode);

        var baseAmount = CalculateBaseAmount(
            billableDuration,
            input.HourlyRate,
            input.HalfHourRate);

        var minimumChargeApplied = 0m;

        if (baseAmount < input.MinimumCharge)
        {
            minimumChargeApplied =
                input.MinimumCharge - baseAmount;

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

            HourlyRateUsed = input.HourlyRate,

            HalfHourRateUsed = input.HalfHourRate,

            BaseAmount = baseAmount,

            MinimumChargeApplied = minimumChargeApplied,

            FullDayMaximumApplied = fullDayMaximumApplied,

            FinalAmount = baseAmount
        };
    }

    private static decimal CalculateBaseAmount(
        TimeSpan billableDuration,
        decimal hourlyRate,
        decimal? halfHourRate)
    {
        var totalMinutes = billableDuration.TotalMinutes;

        if (totalMinutes <= 0)
        {
            return 0m;
        }

 
        if (!halfHourRate.HasValue)
        {
            return
                (decimal)totalMinutes / 60m *
                hourlyRate;
        }

        var fullHours =
            Math.Floor(totalMinutes / 60d);

        var remainingMinutes =
            totalMinutes - (fullHours * 60d);

        var amount =
            (decimal)fullHours * hourlyRate;

        if (remainingMinutes > 0)
        {
            var halfHourUnits =
                Math.Ceiling(remainingMinutes / 30d);

            amount +=
                (decimal)halfHourUnits *
                halfHourRate.Value;
        }

        return amount;
    }

    private static TimeSpan ApplyRounding(
        TimeSpan duration,
        int roundingMinutes,
        RoundingMode roundingMode)
    {
        if (roundingMinutes <= 0 ||
            roundingMode == RoundingMode.None)
        {
            return duration;
        }

        var totalMinutes = duration.TotalMinutes;

        var roundedMinutes = roundingMode switch
        {
            RoundingMode.Up =>
                Math.Ceiling(
                    totalMinutes / roundingMinutes)
                * roundingMinutes,

            RoundingMode.Down =>
                Math.Floor(
                    totalMinutes / roundingMinutes)
                * roundingMinutes,

            _ => totalMinutes
        };

        return TimeSpan.FromMinutes(
            roundedMinutes);
    }
}