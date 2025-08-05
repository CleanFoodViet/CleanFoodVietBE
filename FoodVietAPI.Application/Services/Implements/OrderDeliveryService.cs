using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.OrderDeliveryDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.AccountEnums;
using CleanFoodVietAPI.Data.Enums.OrdeDeliveryEnums;
using CleanFoodVietAPI.Data.Enums.OrderEnums;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class OrderDeliveryService : BaseService<OrderDeliveryService>, IOrderDeliveryService
    {
        public OrderDeliveryService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<OrderDeliveryService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<List<OrderDeliveryDTO>> GetOrderDeliveryInformationOfAOrder(string orderId)
        {
            Ulid orderID = Ulid.Parse(orderId);
            var orderDelivery = await _unitOfWork.GetRepository<OrderDelivery>()
                .GetListAsync(
                    include: odv => odv.Include(x => x.OrderDeliveryDetails).ThenInclude(x => x.Product)
                        .ThenInclude(x => x.ProductPrices.Where(pp => pp.IsCurrent)),
                    predicate: odv => odv.OrderId == orderID,
                    selector: odv => new OrderDeliveryDTO
                    {
                        OrderDeliveryId = odv.OrderDeliveryId,
                        OrderId = odv.OrderId,
                        DeliveryDate = odv.DeliveryDate,
                        DeliveryStatus = odv.DeliveryStatus,
                        CreatedAt = odv.CreatedAt,
                        UpdatedAt = odv.UpdatedAt,
                        Note = odv.Note,
                        OrderDeliveryDetails = _mapper.Map<List<OrderDeliveryDetailDTO>>(odv.OrderDeliveryDetails)
                    }
                );

            return orderDelivery.ToList();
        }

        public async Task CreateOrderDelivery(string orderId, CreateOrderDelieryDTO request)
        {
            Ulid orderID = Ulid.Parse(orderId);
            var order = await _unitOfWork.GetRepository<Order>().GetAsync(predicate: o => o.OrderId == orderID);
            if (order == null) throw new BadHttpRequestException("Order is not found");

            order.Status = OrderStatusEnum.DELIVERING.ToString();
            _unitOfWork.GetRepository<Order>().UpdateAsync(order);

            var orderDelivery = _mapper.Map<OrderDelivery>(request);
            orderDelivery.OrderId = orderID;

            var orderDeliveryDetails = _mapper.Map<List<OrderDeliveryDetail>>(
                    request.delieryDetailsDTOs,
                    opt => opt.Items["OrderDeliveryId"] = orderDelivery.OrderDeliveryId
                );

            await _unitOfWork.GetRepository<OrderDelivery>().InsertAsync(orderDelivery);
            await _unitOfWork.GetRepository<OrderDeliveryDetail>().InsertRangeAsync(orderDeliveryDetails);

            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occur when create Order Delivery (DB query Error)");
        }

        public async Task UpdateOrderDeliveryStatus(string orderId, string orderDeliveryId, string status)
        {
            Ulid orderID = Ulid.Parse(orderId);
            var order = await _unitOfWork.GetRepository<Order>().GetAsync(predicate: o => o.OrderId == orderID);
            if (order == null) throw new BadHttpRequestException("Order is not found");

            Ulid orderDeliveryID = Ulid.Parse(orderDeliveryId);
            var orderDelivery = await _unitOfWork.GetRepository<OrderDelivery>().GetAsync(predicate: odv => odv.OrderDeliveryId == orderDeliveryID);

            if (orderDelivery == null) throw new BadHttpRequestException("Order Delivery is not found");

            if (Enum.TryParse<OrderDeliveryStatusEnum>(status.ToUpper(), out var result))
            {
                orderDelivery.DeliveryStatus = result.ToString();
            }
            else
            {
                throw new BadHttpRequestException("Invalid Order Delivery Status");
            }
            orderDelivery.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.GetRepository<OrderDelivery>().UpdateAsync(orderDelivery);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occur when update the Order Delivery Status (DB query Error)");
        }
    }
}
