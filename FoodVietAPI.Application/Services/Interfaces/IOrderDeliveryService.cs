using CleanFoodVietAPI.Application.DTOs.OrderDeliveryDTOs;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IOrderDeliveryService
    {
        Task<List<OrderDeliveryDTO>> GetOrderDeliveryInformationOfAOrder(string orderId);

        Task CreateOrderDelivery(string orderId, CreateOrderDelieryDTO request);

        Task UpdateOrderDeliveryStatus(string orderId, string orderDeliveryId, string status);
    }
}
