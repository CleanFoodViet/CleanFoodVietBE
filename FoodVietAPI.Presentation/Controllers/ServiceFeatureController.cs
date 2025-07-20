using CleanFoodVietAPI.Application.DTOs.ServiceFeatureDTOs;
using CleanFoodVietAPI.Application.Exceptions;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Exceptions;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

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
        [ProducesResponseType(typeof(IPaginate<ServiceFeatureDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Returns a paginated list of Service Feature with metadata including filtering and sorting parameters")]
        public async Task<IActionResult> GetServiceFeatureList(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] string? filterField = null,
            [FromQuery] string? filterValue = null,
            [FromQuery] string? sortField = null,
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] string? search = null)
        {
            // 1) Validate incoming filterField & sortField
            var validationError = ValidateFilterAndSort<ServiceFeature>(filterField, sortField);
            if (validationError != null)
                return validationError;

            // 2) Fetch paged data
            var features = await _serviceFeatureService
                .GetServiceFeatureList(page, size, filterField, filterValue, sortField, sortOrder, search);

            // 3) Shape response
            var response = new
            {
                page = features.Page,
                size = features.Size,
                total = features.Total,
                totalPages = features.TotalPages,
                filterField = filterField ?? string.Empty,
                filterValue = filterValue ?? string.Empty,
                sortField = sortField ?? string.Empty,
                sortOrder,
                search = search ?? string.Empty,
                items = features.Items
            };

            return Ok(response);
        }

        // GET: api/v1/admin/service-features/{id}
        // Returns detailed information about a specific service feature by ID.
        [HttpGet(ApiEndpointConstant.ServiceFeature.ServiceFeatureDetailEndpoint)]
        [ProducesResponseType(typeof(ServiceFeatureDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Returns detailed information about a specific service feature by ID.")]
        public async Task<IActionResult> GetServiceFeatureDetail([FromQuery] string id)
        {
            try
            {
                var dto = await _serviceFeatureService.GetServiceFeatureDetailAsync(id);
                return Ok(dto);
            }
            catch (DomainValidationException dv)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid ID",
                    Detail = dv.Message,
                    Extensions = { ["errorCode"] = "InvalidFeatureId" }
                });
            }
            catch (NotFoundException nf)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Feature not found",
                    Detail = nf.Message
                });
            }
        }

        // PATCH: api/v1/admin/service-features/{id}
        // Update service feature information (including soft-delete by setting status to INACTIVE)
        [HttpPatch(ApiEndpointConstant.ServiceFeature.UpdateServiceFeatureEndpoint)]
        [ProducesResponseType(typeof(ServiceFeatureDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateServiceFeature(
            [FromQuery, Required] string id,
            [FromBody] UpdateServiceFeatureDTO dto)
        {
            // 1) Parse the ULID
            if (!Ulid.TryParse(id, out var featureId))
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid feature id",
                    Detail = $"'{id}' is not a valid ULID.",
                    Extensions = { ["errorCode"] = "InvalidFeatureId" }
                });
            }

            try
            {
                // 2) Delegate to service
                var updated = await _serviceFeatureService.UpdateServiceFeature(featureId, dto);
                return Ok(updated);
            }
            catch (DeletionRestrictedException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Feature cannot be disabled",
                    Detail = ex.Message,
                    Extensions = { ["errorCode"] = "FeatureInActivePackage" }
                });
            }
            catch (UpdateRestrictedException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid status value",
                    Detail = ex.Message,
                    Extensions = { ["errorCode"] = "InvalidFeatureStatus" }
                });
            }
            catch (ForbiddenException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ProblemDetails
                {
                    Status = StatusCodes.Status403Forbidden,
                    Title = "Operation forbidden",
                    Detail = ex.Message,
                    Extensions = { ["errorCode"] = "Forbidden" }
                });
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Feature not found",
                    Detail = knf.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating feature {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Unexpected error",
                    Detail = ex.Message
                });
            }
        }

        // POST: api/v1/admin/service-features
        // Create a new service feature
        [HttpPost(ApiEndpointConstant.ServiceFeature.ServiceFeaturesEndpoint)]
        [ProducesResponseType(typeof(ServiceFeatureDTO), StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Create a new service feature")]
        public async Task<IActionResult> CreateServiceFeature([FromBody] CreateServiceFeatureDTO createDto)
        {
            var createdFeature = await _serviceFeatureService.CreateServiceFeature(createDto);
            return StatusCode(StatusCodes.Status201Created, createdFeature);
        }

        // Soft Delete endpoint: changes status to INACTIVE after checking for active usage.
        [HttpPatch(ApiEndpointConstant.ServiceFeature.DisableServiceFeatureEndpoint)]
        [ProducesResponseType(typeof(ServiceFeatureDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Soft Delete Service Feature: changes status to INACTIVE after checking for active usage.")]
        public async Task<IActionResult> SoftDeleteServiceFeature([FromQuery] string id)
        {
            try
            {
                var dto = await _serviceFeatureService.SoftDeleteServiceFeature(id);
                return Ok(dto);
            }
            catch (NotFoundException nf)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Feature not found",
                    Detail = nf.Message
                });
            }
            catch (DomainValidationException ex)
            {
                // Return a JSON error response
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = "Feature cannot be disabled",
                    Detail = ex.Message,
                    Extensions = { ["errorCode"] = "FeatureInActivePackage" }
                });
            }
        }
    }
}
