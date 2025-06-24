using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanFoodVietAPI.Data.Entities
{
    public class SubscriptionContractBenefit
    {
        [Key]
        public Ulid SubscriptionContractBenefitId { get; set; } = Ulid.NewUlid();

        [Required]
        public Ulid SubscriptionContractId { get; set; }

        // Benefit type, e.g., "POST_QUOTA", "IMAGE_LIMIT", "POST_PRIORITY", etc.
        [Required]
        [MaxLength(50)]
        public string BenefitType { get; set; }

        // The default benefit value when the subscription is purchased.
        [Required]
        public int DefaultValue { get; set; }

        // The current remaining value updated as the benefit is used.
        [Required]
        public int RemainingValue { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property (using 'virtual' for consistency with other entities)
        [ForeignKey(nameof(SubscriptionContractId))]
        public virtual SubscriptionContract SubscriptionContract { get; set; }
    }
}
