using CleanFoodVietAPI.Application.DTOs.CartDTOs;
using CleanFoodVietAPI.Application.DTOs.OrderDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class OrderController : BaseController<OrderController>
    {
        private readonly IOrderService _orderService;
        public OrderController(ILogger<OrderController> logger, IOrderService orderService) : base(logger)
        {
            _orderService = orderService;
        }

        [HttpGet(ApiEndpointConstant.Account.AccountOrdersEndpoint)]
        [ProducesResponseType(typeof(IPaginate<OrderListDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get Account's Order List (Include Retailer and Gardener base on account id input)")]
        public async Task<IActionResult> GetAccountOrderList(
            [FromRoute]string id, 
            [FromQuery]int page = 1, 
            [FromQuery]int size = 10, 
            [FromQuery] string? filterField = null,
            [FromQuery] string? filterValue = null)
        {
            var res = await _orderService.GetAccountOrderList(id, page, size, filterField, filterValue);
            return Ok(res);
        }


        [HttpGet(ApiEndpointConstant.Account.AccountOrderEndpoint)]
        [ProducesResponseType(typeof(OrderDTO), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get Account's Order Information")]
        public async Task<IActionResult> GetOrderInformation([FromRoute]string id, [FromRoute] string orderId)
        {
            var res = await _orderService.GetOrderInformation(orderId, id);

            return Ok(res);
        }

        [HttpPost(ApiEndpointConstant.Order.OrderEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Create Order from Retailer Carts (the request is the Information in the CartDTO which use to show the Cart List in UI of Retailer)")]
        public async Task<IActionResult> CreateOrder(
            [FromQuery] string paymentMethod,
            [FromQuery] string shippingAddress,
            [FromBody]List<CartOrderDTO> request)
        {
            await _orderService.CreateOrder(request, paymentMethod, shippingAddress);

            return StatusCode(StatusCodes.Status201Created, "Create order successfully");
        }

        [HttpPatch(ApiEndpointConstant.Order.OrderShippingCostEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update the order shipping cost")]
        public async Task<IActionResult> UpdateOrderShippingCost([FromRoute] string id, [FromQuery] decimal shippingCost)
        {
            await _orderService.UpdateOrderShippingCost(id, shippingCost);

            return Ok("Update order shipping cost successfully");
        }

        [HttpPatch(ApiEndpointConstant.Order.OrderEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = @"Update Order Status. Order Status: 
            PENDING (Created but not approve/reject), PREPARING (after approve), 
            DELIVERING (When a Order Detail Quantity is Delivered partly), 
            COMPLETED (when all Order Detail Quantity are delivered)), 
            CANCELLED (be rejected by Gardener or cancelled by Retailer before Gardener Approve/Reject)")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] string id, [FromQuery] string status)
        {
            await _orderService.UpdateOrderStatus(id, status);

            return Ok("Update order status successfully");
        }

        [HttpPatch(ApiEndpointConstant.Order.OrderDeatilCheckEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = @"Check and update the Order detail and Order Status")]
        public async Task<IActionResult> CheckAndUpdateOrderDetailStatus([FromRoute] string id, [FromBody] List<CheckOrderDetailDeliveryDTO> request)
        {
            await _orderService.UpdateOrderDetailDeliveryStatus(id, request);

            return Ok("Update order detail status successfully");
        }

        [HttpPatch(ApiEndpointConstant.Order.OrderRejectEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update the order shipping cost")]
        public async Task<IActionResult> AddRejectrReason([FromRoute] string id, [FromBody] string reason)
        {
            await _orderService.CancelOrder(id, reason);

            return Ok("Reject order successfully");
        }
    }
}
