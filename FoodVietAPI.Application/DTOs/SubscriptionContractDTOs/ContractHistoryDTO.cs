using System;

namespace CleanFoodVietAPI.Application.DTOs.SubscriptionContractDTOs
{
    public record ContractHistoryDTO(
        Ulid SubscriptionId,
        string Status,
        string SubscriptionType,
        string GardenerPhoneNumber,
        string ServicePackageName,
        decimal ServicePackagePrice
    );
}
