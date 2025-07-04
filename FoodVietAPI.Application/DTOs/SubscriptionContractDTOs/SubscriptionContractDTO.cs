using CleanFoodVietAPI.Application.DTOs.SubscriptionContractBenefitDTOs;

namespace CleanFoodVietAPI.Application.DTOs.SubscriptionContractDTOs
{
    public class SubscriptionContractDTO
    {
        public Ulid SubscriptionId { get; set; }
        public Ulid GardenerId { get; set; }

        // NEW: Gardener info
        public string GardenerName { get; set; } = null!;
        public string GardenerEmail { get; set; } = null!;
        public string GardenerPhone { get; set; } = null!;

        public Ulid ServicePackageId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = null!;
        public string SubscriptionType { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public List<SubscriptionContractBenefitDTO> Benefits { get; set; } = new();
    }
}

