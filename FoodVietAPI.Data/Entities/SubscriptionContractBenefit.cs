using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class SubscriptionContractBenefit
    {
        [Key]
        public Ulid SubscriptionContractBenefitId { get; set; } = Ulid.NewUlid();

        [Required]
        public Ulid SubscriptionContractId { get; set; }

        // Benefit type, e.g., "POST_QUOTA", "IMAGE_LIMIT", "POST_PRIORITY", etc.
        public string BenefitType { get; set; } = null!;

        // The default benefit value when the subscription is purchased.
        public int DefaultValue { get; set; }

        // The current remaining value updated as the benefit is used.
        public int RemainingValue { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property (using 'virtual' for consistency with other entities)
        public virtual SubscriptionContract SubscriptionContract { get; set; } = null!;
    }
}
