using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.CartDTOs;
using CleanFoodVietAPI.Application.DTOs.OrderDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Application.Specifications;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.AccountEnums;
using CleanFoodVietAPI.Data.Enums.OrderEnums;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class OrderService : BaseService<OrderService>, IOrderService
    {
        public OrderService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<OrderService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<IPaginate<OrderListDTO>> GetAccountOrderList(string accountId, int page, int size, string? filterField, string? filterValue)
        {
            OrderSpecification spec = new OrderSpecification(filterField, filterValue, "CreatedAt", "desc");

            Ulid accId = Ulid.Parse(accountId);
            var orders = await _unitOfWork.GetRepository<Order>()
                .GetPagingListAsync(
                    include: o => o.Include(x => x.OrderDetails).Include(x => x.Retailer),
                    predicate: o => o.GardenerId == accId || o.RetailerId == accId,
                    selector: o => new OrderListDTO(
                        o.OrderId,
                        o.RetailerId,
                        o.Retailer.Name,
                        o.GardenerId,
                        o.Status,
                        o.TotalAmount,
                        o.CreatedAt,
                        o.OrderDetails.Count()),
                    spec: spec,
                    page: page, size: size
                );

            return orders;
        }

        public async Task<OrderDTO> GetOrderInformation(string orderId, string accountId)
        {
            Ulid accId = Ulid.Parse(accountId);
            var account = await _unitOfWork.GetRepository<Account>()
                .GetAsync(include: acc => acc.Include(x => x.Role),
                          predicate: acc => acc.AccountId == accId);
            if (account == null) throw new BadHttpRequestException("Account is not found");

            Ulid orderID = Ulid.Parse(orderId);
            var orderInfo = await _unitOfWork.GetRepository<Order>()
                .GetAsync(
                    include: o => o.Include(x => x.OrderDetails),
                    predicate: o => o.OrderId == orderID,
                    selector: o => new OrderDTO
                    {
                        OrderId = o.OrderId,
                        RetailerId = o.RetailerId,
                        GardenerId = o.GardenerId,
                        AccountName = account.Role.Name == AccountRoleEnum.RETAILER.ToString() ?
                                        o.Gardener.Name : o.Retailer.Name,
                        Status = o.Status,
                        TotalAmount = o.TotalAmount,
                        PaymentMethod = o.PaymentMethod,
                        CreatedAt = o.CreatedAt
                    }
                );
            if (orderInfo == null) throw new BadHttpRequestException("Order is not found");

            var orderDetails = await _unitOfWork.GetRepository<OrderDetail>()
                .GetListAsync(
                    include: od => od.Include(x => x.Product)
                                     .ThenInclude(x => x.OrderDeliveryDetails.Where(odd => odd.ProductId == x.ProductId))
                                     .Include(x => x.Product.ProductPrices.Where(pp => pp.IsCurrent)),
                    predicate: od => od.OrderId == orderID,
                    selector: od => new OrderDetailDTO
                    {
                         OrderDetailId = od.OrderDetailId,
                         Price = od.Price,
                         Quantity = od.Quantity,
                         ProductUnit = od.ProductUnit,
                         DeliveryStatus = od.DeliveryStatus,
                         ProductId = od.ProductId,
                         ProductName = od.Product.ProductName,
                         WeightUnit = od.Product.ProductPrices.First().WeightUnit,
                         Currency = od.Product.ProductPrices.First().Currency,
                         DeliveredQuantity = od.Product.OrderDeliveryDetails.Sum(x => x.Quantity)
                    }
                );

            orderInfo.orderDetails = orderDetails.ToList();

            return orderInfo;
        }

        public async Task CreateOrder(List<CartDTO> carts, string paymentMethod)
        {
            foreach(var cart in carts)
            {
                var order = _mapper.Map<Order>(cart);
                order.PaymentMethod = paymentMethod;
                order.TotalAmount = cart.CartItems.Sum(ct => ct.Price * ct.Quantity); //Need to detailed discuss.

                var orderDetails = _mapper.Map<List<OrderDetail>>(
                        cart.CartItems,
                        opt => opt.Items["OrderId"] = order.OrderId //Take the Id of newly created Order above and asign the value to OrderId Field
                    );

                await _unitOfWork.GetRepository<Order>().InsertAsync(order);
                await _unitOfWork.GetRepository<OrderDetail>().InsertRangeAsync(orderDetails);
            }

            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occur when create Order (DB query Error)");
        }

        public async Task UpdateOrderStatus(string orderId, string status)
        {
            Ulid orderID = Ulid.Parse(orderId);
            var order = await _unitOfWork.GetRepository<Order>()
                .GetAsync(predicate: o => o.OrderId == orderID);
            if (order == null) throw new BadHttpRequestException("Order is not found");

            if (Enum.TryParse<OrderStatusEnum>(status.ToUpper(), out var result))
            {
                order.Status = result.ToString();
            }
            else
            {
                throw new BadHttpRequestException("Invalid order status");
            }

            _unitOfWork.GetRepository<Order>().UpdateAsync(order);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occur when update the Order Status (DB query Error)");
        }

        public async Task UpdateOrderDetailDeliveryStatus(string orderId, List<CheckOrderDetailDeliveryDTO> checkOrderDetails)
        {
            Ulid orderID = Ulid.Parse(orderId);
            var order = await _unitOfWork.GetRepository<Order>()
                .GetAsync(predicate: o => o.OrderId == orderID);
            if (order == null) throw new BadHttpRequestException("Order is not found");

            var orderDetails = await _unitOfWork.GetRepository<OrderDetail>().GetListAsync(predicate: od => od.OrderId == orderID);

            bool isRemainDeliveringZero = false;
            var updatedOrderDetails = new List<OrderDetail>();

            foreach (var orderDetail in orderDetails)
            {
                var match = checkOrderDetails.FirstOrDefault(cd => cd.OrderDetailId == orderDetail.OrderDetailId);
                if (match != null)
                {
                    orderDetail.DeliveryStatus = match.RemainDeliveryQuantity == 0 
                        ? OrderDetailStatusEnum.DELIVERIED.ToString() : OrderDetailStatusEnum.DELIVERING.ToString();
                    updatedOrderDetails.Add(orderDetail);

                    if (!isRemainDeliveringZero && match.RemainDeliveryQuantity == 0) isRemainDeliveringZero = true;
                }
            }

            _unitOfWork.GetRepository<OrderDetail>().UpdateRange(updatedOrderDetails);

            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occur when update the Order Detail Status (DB query Error)");

            if (isRemainDeliveringZero)
            {
                var deliveringOrderDetails = await _unitOfWork.GetRepository<OrderDetail>()
                    .GetListAsync(predicate: od => od.OrderId == orderID && od.DeliveryStatus == OrderDetailStatusEnum.DELIVERING.ToString());
                if(deliveringOrderDetails == null || deliveringOrderDetails.Count == 0)
                {
                    order.Status = OrderStatusEnum.DELIVERED.ToString();
                    bool isSuccessUpdate = await _unitOfWork.CommitAsync() > 0;
                    if (!isSuccessUpdate) throw new Exception("Error occur when update the Order Status (DB query Error)");
                }
            }
        }
    }
}
