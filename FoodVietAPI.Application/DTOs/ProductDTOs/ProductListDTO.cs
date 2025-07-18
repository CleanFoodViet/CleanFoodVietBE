namespace CleanFoodVietAPI.Application.DTOs.ProductDTOs
{
    public record ProductListDTO
    (
        Ulid ProductId,
        string ProductName,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        string Status,
        string ProductCategory,
        decimal Price,
        string Currency,
        string WeightUnit
    );
}
