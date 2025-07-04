namespace CleanFoodVietAPI.Application.DTOs.SubscriptionOrderPaymentDTOs
{
    public class SubscriptionOrderPaymentDTO
    {
        public Ulid SubscriptionOrderPaymentId { get; set; }
        public decimal PaymentAmount { get; set; }
        public string Currency { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
    }
}
