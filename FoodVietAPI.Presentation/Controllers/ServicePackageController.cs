using CleanFoodVietAPI.Application.DTOs.ServicePackageDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using static CleanFoodVietAPI.Presentation.Constants.ApiEndpointConstant;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class ServicePackageController : BaseController<ServicePackageController>
    {
        private readonly IServicePackageService _servicePackageService;

        public ServicePackageController(
            ILogger<ServicePackageController> logger,
            IServicePackageService servicePackageService)
            : base(logger)
        {
            _servicePackageService = servicePackageService;
        }

        // GET: api/v1/admin/service-packages
        // Returns a paginated list of service packages with metadata including filtering and sorting parameters.
        [HttpGet(ApiEndpointConstant.ServicePackageEndpoint.ServicePackagesEndpoint)]
        [ProducesResponseType(typeof(IPaginate<ServicePackageDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetServicePackages(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] string? filterField = null,
            [FromQuery] string? filterValue = null,
            [FromQuery] string? sortField = null,
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] string? search = null)
        {
            // 1) Validate filter & sort fields
            var validationError = ValidateFilterAndSort<ServicePackage>(filterField, sortField);
            if (validationError != null)
                return validationError;

            // 2) Fetch paged packages (with features)
            var packagesPage = await _servicePackageService
                .GetServicePackagesWithFeaturesAsync(
                    page, size, filterField, filterValue, sortField, sortOrder, search);

            // 3) Shape response
            var response = new
            {
                page = packagesPage.Page,
                size = packagesPage.Size,
                total = packagesPage.Total,
                totalPages = packagesPage.TotalPages,
                filterField = filterField ?? string.Empty,
                filterValue = filterValue ?? string.Empty,
                sortField = sortField ?? string.Empty,
                sortOrder,
                search = search ?? string.Empty,
                items = packagesPage.Items
            };

            return Ok(response);
        }

        // GET: api/v1/admin/service-packages/{id}
        // Returns detailed information about a specific service package by ID, including its features.
        [HttpGet(ApiEndpointConstant.ServicePackageEndpoint.ServicePackageDetailEndpoint)]
        [ProducesResponseType(typeof(ServicePackageDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetServicePackageDetail([FromRoute] Ulid id)
        {
            var packageDetail = await _servicePackageService.GetServicePackageDetailAsync(id);
            return Ok(packageDetail);
        }

        // POST: api/v1/admin/service-packages
        // Creates a new service package with the provided details.
        [HttpPost(ApiEndpointConstant.ServicePackageEndpoint.ServicePackagesEndpoint)]
        [ProducesResponseType(typeof(ServicePackageDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateServicePackage([FromBody] CreateServicePackageDTO createDto)
        {
            var packageDto = await _servicePackageService.CreateServicePackageAsync(createDto);
            return StatusCode(StatusCodes.Status201Created, packageDto);
        }

        // PATCH endpoint for updating allowed fields (basic info)
        // of the service package, excluding features, which are managed separately.
        [HttpPatch(ServicePackageEndpoint.ServicePackageDetailEndpoint)]
        [ProducesResponseType(typeof(ServicePackageDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateServicePackage(
            [FromRoute] Ulid id,
            [FromBody] UpdateServicePackageDTO updateDto)
        {
            var packageDto = await _servicePackageService.UpdateServicePackageAsync(id, updateDto);
            return Ok(packageDto);
        }

        // PATCH endpoint for soft-deleting (disabling) the package.
        // This endpoint sets the status of the package to INACTIVE.
        [HttpPatch(ServicePackageEndpoint.DisableServicePackageEndpoint)]
        [ProducesResponseType(typeof(ServicePackageDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> DisableServicePackage([FromRoute] Ulid id)
        {
            var packageDto = await _servicePackageService.DisableServicePackageAsync(id);
            return Ok(packageDto);
        }

        // PATCH endpoint for activating the package.
        // This endpoint sets the status of the package to ACTIVE.
        [HttpPatch(ServicePackageEndpoint.ActivateServicePackageEndpoint)]
        [ProducesResponseType(typeof(ServicePackageDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> ActivateServicePackage([FromRoute] Ulid id)
        {
            var packageDto = await _servicePackageService.ActivateServicePackageAsync(id);
            return Ok(packageDto);
        }
    }
}
