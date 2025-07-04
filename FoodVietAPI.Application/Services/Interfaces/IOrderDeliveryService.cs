using CleanFoodVietAPI.Application.DTOs.OrderDeliveryDTOs;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.OrdeDeliveryEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IOrderDeliveryService
    {
        Task<List<OrderDeliveryDTO>> GetOrderDeliveryInformationOfAOrder(string orderId);

        Task CreateOrderDelivery(string orderId, CreateOrderDelieryDTO request);

        Task UpdateOrderDeliveryStatus(string orderId, string orderDeliveryId, string status);
    }
}
