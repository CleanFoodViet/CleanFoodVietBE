namespace CleanFoodVietAPI.Application.DTOs.ProductPriceDTOs
{
    public record ProductPriceDTO
    (
        Ulid ProductPriceId,
        decimal Price,
        string Currency,
        string WeightUnit,
        DateTime AvailabledDate,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        bool IsCurrent
    );
}
