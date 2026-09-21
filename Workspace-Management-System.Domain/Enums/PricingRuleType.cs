using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Domain.Enums
{
    public enum PricingRuleType
    {
        HourlyRate,
        HalfHourRate,
        MinimumCharge,
        RoundUp,
        RoundDown,
        FullDayMaximum,
        PackagePrice,
        ExtraHourPrice,
        WeekendPricing,
        SpecialPricing,
        MemberPricing
    }
}
