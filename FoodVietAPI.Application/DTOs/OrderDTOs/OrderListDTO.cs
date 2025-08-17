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
        decimal ShippingCost,
        DateTime CreatedAt,
        int ProductTypeAmount,
        decimal TotalDepositAmount
    );
}
