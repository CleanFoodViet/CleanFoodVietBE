using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.SubscriptionContractDTOs
{
    public record SubscriptionContractDTO
    (
        Ulid SubscriptionId,
        DateTime StartDate,
        DateTime EndDate,
        string Status,
        string SubscriptionType,
        DateTime CreatedAt,

        string GardenerPhoneNumber,
        string ServicePackageName,
        decimal ServicePackagePrice
    );
}
