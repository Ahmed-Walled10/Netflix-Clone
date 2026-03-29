using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixClone.Application.Features.Subscription_Plans.Queries.GetPlans
{
        public class GetPlansResponce
        {
            public List<SubscriptionPlanDto> Plans { get; set; } = new();
            public bool Success { get; set; }
            public string? Message { get; set; } = string.Empty;
        }

        public class SubscriptionPlanDto
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public string Description { get; set; } = string.Empty;
            public int MaxProfiles { get; set; }
            public string VideoQuality { get; set; } = string.Empty;
        }
    
}
