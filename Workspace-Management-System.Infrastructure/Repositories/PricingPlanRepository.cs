using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Models;
using Workspace_Management_System.Infrastructure.Persistence.Context;
namespace Workspace_Management_System.Infrastructure.Repositories
{
    public class PricingPlanRepository: GenericRepository<PricingPlan>, IPricingPlanRepository
    {
        public PricingPlanRepository(ApplicationDbContext dbContext)
          : base(dbContext)
        {
        }
    }
}
