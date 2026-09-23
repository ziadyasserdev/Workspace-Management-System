using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.PricingRules.Dtos
{
    public class PricingRuleDto
    {
        public int Id { get; set; }

        public int PricingPlanId { get; set; }
        public int WorkspaceTypeId { get; set; }

        public PricingRuleType RuleType { get; set; }
        public decimal? Value { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DayOfWeek? DayOfWeek { get; set; }

        public bool IsActive { get; set; }
    }
}
