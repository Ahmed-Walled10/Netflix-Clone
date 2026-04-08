using NetflixClone.Domain.Common.Enums;

namespace NetflixClone.Application.Features.Subscription_Plans.Commands.UpdatePlans
{
    public class UpdatePlanRequest
    {
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public decimal? Price { get; set; }
        public BillingPeriod? BillingPeriod { get; set; }
        public int? MaxProfiles { get; set; }
        public VideoQuality? MaxVideoQuality { get; set; }
        public string? StripePriceId { get; set; }
        public bool? IsActive { get; set; }
    }
}
