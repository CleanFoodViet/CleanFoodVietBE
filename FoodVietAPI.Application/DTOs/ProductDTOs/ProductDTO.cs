namespace CleanFoodVietAPI.Application.DTOs.ProductDTOs
{
    public record ProductDTO
    (
        //Product Data Field
        Ulid ProductId,
        string ProductName,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        string Status,
        string ProductCategory,
        string HarvestStatus,

        List<string> ProductTags,

        //Product Price Data Field
        Ulid ProductPriceId,
        decimal Price,  
        string Currency,
        DateTime AvailabledDate,
        string WeightUnit,

        //Gardener Field
        Ulid GardenerId,
        string GardenerName
    );
}
