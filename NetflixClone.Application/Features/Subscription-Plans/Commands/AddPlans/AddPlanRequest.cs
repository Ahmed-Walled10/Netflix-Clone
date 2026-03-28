
using MediatR;
using NetflixClone.Domain.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace NetflixClone.Application.Features.Subscription_Plans.Commands.AddPlans
{
    public class AddPlanRequest : IRequest<AddPlanResponse>
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? DisplayName { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public BillingPeriod BillingPeriod { get; set; }
        [Required]
        public int MaxProfiles { get; set; }
        [Required]
        public VideoQuality MaxVideoQuality { get; set; }

        public string? StripePriceId { get; set; }
        public bool? IsActive { get; set; } = true;


    }
}
