using CleanFoodVietAPI.Application.DTOs.SubscriptionOrderPaymentDTOs;

namespace CleanFoodVietAPI.Application.DTOs.SubscriptionOrderDTOs
{
    public class ServicePackageOrderDTO
    {
        public Ulid ServicePackageOrderId { get; set; }
        public Ulid GardenerId { get; set; }

        // NEW: Gardener info
        public string GardenerName { get; set; } = null!;
        public string GardenerEmail { get; set; } = null!;
        public string GardenerPhone { get; set; } = null!;

        public Ulid ServicePackageId { get; set; }
        public string? ServicePackageName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public List<SubscriptionOrderPaymentDTO> Payments { get; set; } = new();
    }
}

