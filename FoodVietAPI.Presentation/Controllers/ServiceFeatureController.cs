using CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTO;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class ServiceFeatureController : BaseController<ServiceFeatureController>
    {
        private readonly IServiceFeatureService _serviceFeatureService;

        public ServiceFeatureController(
            ILogger<ServiceFeatureController> logger,
            IServiceFeatureService serviceFeatureService)
            : base(logger)
        {
            _serviceFeatureService = serviceFeatureService;
        }

        // GET: api/v1/admin/service-features
        // Returns a paginated list with metadata including filtering and sorting parameters.
        [HttpGet(ApiEndpointConstant.ServiceFeature.ServiceFeaturesEndpoint)]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetServiceFeatureList(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] string? filterField = null,
            [FromQuery] string? filterValue = null,
            [FromQuery] string? sortField = null,
            [FromQuery] string? sortOrder = "asc")
        {
            var features = await _serviceFeatureService
                .GetServiceFeatureList(page, size, filterField, filterValue, sortField, sortOrder);

            // Wrap response with additional metadata.
            var response = new
            {
                page = features.Page,
                size = features.Size,
                total = features.Total,
                totalPages = features.TotalPages,
                filterField = filterField ?? string.Empty,
                filterValue = filterValue ?? string.Empty,
                sortField = sortField ?? string.Empty,
                sortOrder = sortOrder,
                items = features.Items
            };

            return Ok(response);
        }

        // PATCH: api/v1/admin/service-features/{id}
        // Update service feature information (including soft-delete by setting status to INACTIVE)
        [HttpPatch(ApiEndpointConstant.ServiceFeature.ServiceFeatureEndpoint)]
        [ProducesResponseType(typeof(ServiceFeatureDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateServiceFeature(
            [FromRoute] Ulid id,
            [FromBody] UpdateServiceFeatureDTO updateDto)
        {
            var updatedFeature = await _serviceFeatureService.UpdateServiceFeature(id, updateDto);
            return Ok(updatedFeature);
        }

        // POST: api/v1/admin/service-features
        // Create a new service feature
        [HttpPost(ApiEndpointConstant.ServiceFeature.ServiceFeaturesEndpoint)]
        [ProducesResponseType(typeof(ServiceFeatureDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateServiceFeature([FromBody] CreateServiceFeatureDTO createDto)
        {
            var createdFeature = await _serviceFeatureService.CreateServiceFeature(createDto);
            return StatusCode(StatusCodes.Status201Created, createdFeature);
        }
    }
}
