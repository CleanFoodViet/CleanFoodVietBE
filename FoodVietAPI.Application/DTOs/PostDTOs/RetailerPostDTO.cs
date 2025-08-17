using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.PostDTOs
{
    public record RetailerPostDTO
    (
        Ulid PostId,
        string Title,
        decimal Price,
        string Currency,
        string ThumbNail,
        Ulid GardenerId,
        string GardenerName,
        string GardenerAvatar,
        DateTime CreatedAt,
        string Content,
        string Status,
        decimal Rating,
        string WeightUnit,
        bool hasProductCertificate,
        Ulid ProductId,
        string ProductCategory,
        string HarvestStatus
    );
}
