using CleanFoodVietAPI.Application.DTOs.OrderDeliveryDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class OrderDeliveryController : BaseController<OrderDeliveryController>
    {
        private readonly IOrderDeliveryService _orderDeliveryService;
        public OrderDeliveryController(ILogger<OrderDeliveryController> logger, IOrderDeliveryService orderDeliveryService) : base(logger)
        {
            _orderDeliveryService = orderDeliveryService;
        }

        [HttpGet(ApiEndpointConstant.Order.OrderDeliveriesEndpoint)]
        [ProducesResponseType(typeof(List<OrderDeliveryDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all the delivery information of a specific order")]
        public async Task<IActionResult> GetOrderDeliveryOfAOrder([FromRoute]string id)
        {
            var res = await _orderDeliveryService.GetOrderDeliveryInformationOfAOrder(id);

            return Ok(res);
        }

        [HttpPost(ApiEndpointConstant.Order.OrderDeliveriesEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Create a order delivery for a specific order")]
        public async Task<IActionResult> CreateOrderDelivery([FromRoute] string id, [FromBody] CreateOrderDelieryDTO request)
        {
            await _orderDeliveryService.CreateOrderDelivery(id, request);

            return StatusCode(StatusCodes.Status201Created, "Create order delivery successfully");
        }

        [HttpPatch(ApiEndpointConstant.Order.OrderDeliveryEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Updare delivery order stauts (Order Delivery Status: DELIVERING (when delivery in process), DELIVERED (when the delivery is done))")]
        public async Task<IActionResult> UpdateOrderDeliveryStatus([FromRoute] string id, [FromRoute]string orderDeliveryId, [FromQuery]string status)
        {
            await _orderDeliveryService.UpdateOrderDeliveryStatus(id, orderDeliveryId, status);

            return Ok("Update OrderDelivery status Successfully");
        }
    }
}
