using CleanFoodVietAPI.Application.DTOs.CartDTOs;
using CleanFoodVietAPI.Application.DTOs.OrderDTOs;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Paginate;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IPaginate<OrderListDTO>> GetAccountOrderList(string accountId, int page, int size, string? filterField, string? filterValue);
        Task<OrderDTO> GetOrderInformation(string orderId, string accountId);
        Task CreateOrder(List<CartDTO> carts, string paymentMethod);
        Task UpdateOrderShippingCost(string orderId, decimal shippingCost);
        Task UpdateOrderStatus(string orderId, string status);
        Task UpdateOrderDetailDeliveryStatus(string orderId, List<CheckOrderDetailDeliveryDTO> checkOrderDetails);
    }
}
