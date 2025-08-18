namespace CleanFoodVietAPI.Application.DTOs.SubscriptionContractDTOs
{
    public record CurrentSubscriptionContractBenefitDTO
    {
        public int DefaultValue { get; set; }
        public int RemainingValue { get; set; }
    }
}
