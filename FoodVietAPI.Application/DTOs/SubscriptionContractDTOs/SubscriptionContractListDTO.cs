using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
