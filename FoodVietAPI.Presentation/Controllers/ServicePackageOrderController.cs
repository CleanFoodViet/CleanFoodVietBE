using CleanFoodVietAPI.Application.DTOs.SubscriptionOrderDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class ServicePackageOrderController : BaseController<ServicePackageOrderController>
    {
        private readonly IServicePackageOrderService _svc;

        public ServicePackageOrderController(
            ILogger<ServicePackageOrderController> logger,
            IServicePackageOrderService svc)
            : base(logger)
        {
            _svc = svc;
        }

        // GET /api/v1/admin/service-package-orders
        [HttpGet(ApiEndpointConstant.ServicePackageOrder.ServicePackageOrdersEndpoint)]
        [ProducesResponseType(typeof(IPaginate<ServicePackageOrderDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get Subscription Order List")]
        public async Task<IActionResult> GetOrders(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] string? filterField = null,
            [FromQuery] string? filterValue = null,
            [FromQuery] string? sortField = null,
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] string? search = null)
        {
            // 1) Validate filter and sort fields
            var validationError = ValidateFilterAndSort<ServicePackageOrder>(
            filterField, sortField);
            if (validationError != null)
                return validationError;

            // 2) Fetch paged data
            var orders = await _svc.GetOrdersAsync(
                page, size, filterField, filterValue, sortField, sortOrder, search);

            // 3) Shape response
            var response = new
            {
                page = orders.Page,
                size = orders.Size,
                total = orders.Total,
                totalPages = orders.TotalPages,
                filterField = filterField ?? string.Empty,
                filterValue = filterValue ?? string.Empty,
                sortField = sortField ?? string.Empty,
                sortOrder = sortOrder,
                search = search ?? string.Empty,
                items = orders.Items
            };

            return Ok(response);
        }

        // GET /api/v1/admin/service-package-orders/{id}
        [HttpGet(ApiEndpointConstant.ServicePackageOrder.ServicePackageOrderDetailEndpoint)]
        [ProducesResponseType(typeof(ServicePackageOrderDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get Subscription Order Detail")]
        public async Task<IActionResult> GetOrderDetail(
        [FromQuery, Required] string id)
        {
            // 1) Validate & parse
            if (!Ulid.TryParse(id, out var orderId))
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid order id",
                    Detail = $"'{id}' is not a valid Ulid.",
                    Extensions = { ["errorCode"] = "InvalidOrderId" }
                });
            }

            // 2) Fetch & catch not-found
            try
            {
                var dto = await _svc.GetOrderDetailAsync(orderId);
                return Ok(dto);
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Order not found",
                    Detail = knf.Message
                });
            }
        }
    }

}
