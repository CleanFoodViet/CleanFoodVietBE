namespace CleanFoodVietAPI.Application.DTOs.PostDTOs
{
    public record PostListDTO
    (
        Ulid PostId,
        string Title,
        decimal Price,
        string Currency,
        string ThumbNail,
        string GardenerName,
        string GardenerAvatar,
        DateTime CreatedAt,
        string Content,
        string Status,
        decimal Rating,
        string WeightUnit,
        bool hasProductCertificate,
        string HarvestStatus,
        decimal DepositAmount,
        int DepositPercentage
    );
}
