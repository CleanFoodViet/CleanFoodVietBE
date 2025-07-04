namespace CleanFoodVietAPI.Application.DTOs.SubscriptionContractDTOs
{
    public record SubscriptionContractListDTO
    (
        Ulid SubscriptionId,
        string Status,
        string SubscriptionType,

        string GardenerPhoneNumber,
        string ServicePackageName,
        decimal ServicePackagePrice
    );
}
