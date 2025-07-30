namespace CleanFoodVietAPI.Application.DTOs.OrderDTOs
{
    public record OrderListDTO
    (
        Ulid OrderId,
        Ulid RetailerId,
        string RetailerName,
        Ulid GardenerId,
        string Status,
        decimal TotalAmount,
        DateTime CreatedAt,
        int ProductTypeAmount
    );
}
