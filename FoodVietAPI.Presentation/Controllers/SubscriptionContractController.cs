using CleanFoodVietAPI.Application.DTOs.SubscriptionContractDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class SubscriptionContractController : BaseController<SubscriptionContractController>
    {
        private readonly ISubscriptionContractService _svc;

        public SubscriptionContractController(
            ILogger<SubscriptionContractController> logger,
            ISubscriptionContractService svc)
            : base(logger)
        {
            _svc = svc;
        }

        // GET /api/v1/admin/subscription-contracts
        [HttpGet(ApiEndpointConstant.SubscriptionContract.SubscriptionContractsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<SubscriptionContractDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get Subscription Contract List")]
        public async Task<IActionResult> GetContracts(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] string? filterField = null,
            [FromQuery] string? filterValue = null,
            [FromQuery] string? sortField = null,
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] string? search = null)
        {
            // 1) Validate incoming filterField & sortField
            var validationError = ValidateFilterAndSort<SubscriptionContract>(
            filterField, sortField);
            if (validationError != null)
                return validationError;

            // 2) Fetch paged data
            var contracts = await _svc.GetContractsAsync(
                page, size, filterField, filterValue, sortField, sortOrder, search);

            // 3) Shape response
            var response = new
            {
                page = contracts.Page,
                size = contracts.Size,
                total = contracts.Total,
                totalPages = contracts.TotalPages,
                filterField = filterField ?? string.Empty,
                filterValue = filterValue ?? string.Empty,
                sortField = sortField ?? string.Empty,
                sortOrder = sortOrder,
                search = search ?? string.Empty,
                items = contracts.Items
            };

            return Ok(response);
        }

        // GET /api/v1/admin/subscription-contracts/{id}
        [HttpGet(ApiEndpointConstant.SubscriptionContract.SubscriptionContractEndpoint)]
        [ProducesResponseType(typeof(SubscriptionContractDTO), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get Subscription Contract Detail")]
        public async Task<IActionResult> GetContractDetail([FromRoute] Ulid id)
        {
            var dto = await _svc.GetContractDetailAsync(id);
            return Ok(dto);
        }
    }

}
