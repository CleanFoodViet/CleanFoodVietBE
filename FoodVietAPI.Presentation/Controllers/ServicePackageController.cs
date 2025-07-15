using CleanFoodVietAPI.Application.DTOs.ServicePackageDTOs;
using CleanFoodVietAPI.Application.Exceptions;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

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
        [SwaggerOperation(Summary = "Returns a paginated list of service packages with metadata including filtering and sorting parameters.")]
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
        [SwaggerOperation(Summary = "Returns detailed information about a specific service package by ID, including its features.")]
        public async Task<IActionResult> GetServicePackageDetail([FromQuery] string id)
        {
            try
            {
                var dto = await _servicePackageService.GetServicePackageDetailAsync(id);
                return Ok(dto);
            }
            catch (DomainValidationException dv)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Package ID",
                    Detail = dv.Message,
                    Extensions = { ["errorCode"] = "InvalidPackageId" }
                });
            }
            catch (NotFoundException nf)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Package not found",
                    Detail = nf.Message
                });
            }
        }

        // POST: api/v1/admin/service-packages
        // Creates a new service package with the provided details.
        [HttpPost(ApiEndpointConstant.ServicePackageEndpoint.ServicePackagesEndpoint)]
        [ProducesResponseType(typeof(ServicePackageDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Creates a new service package with the provided details.")]
        public async Task<IActionResult> CreateServicePackage([FromBody] CreateServicePackageDTO createDto)
        {
            try
            {
                var dto = await _servicePackageService.CreateServicePackageAsync(createDto);
                // return 201 + the created object
                return CreatedAtAction(
                    nameof(GetServicePackageDetail),
                    new { id = dto.ServicePackageId },
                    dto);
            }
            catch (DomainValidationException dv)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid service package",
                    Detail = dv.Message,
                    Extensions = { ["errorCode"] = "PackageCreateFailed" }
                });
            }
        }

        // PATCH endpoint for updating allowed fields (basic info)
        // of the service package, excluding features, which are managed separately.
        [HttpPatch(ApiEndpointConstant.ServicePackageEndpoint.ServicePackageDetailEndpoint)]
        [ProducesResponseType(typeof(ServicePackageDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Updating allowed fields (basic info) of the service package, excluding features, which are managed separately.")]
        public async Task<IActionResult> UpdateServicePackage(
            [FromBody] UpdateServicePackageDTO updateDto)
        {
            try
            {
                var dto = await _servicePackageService.UpdateServicePackageAsync(updateDto);
                return Ok(dto);
            }
            catch (DomainValidationException dv)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid input",
                    Detail = dv.Message,
                    Extensions = { ["errorCode"] = "PackageUpdateFailed" }
                });
            }
            catch (NotFoundException nf)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Package not found",
                    Detail = nf.Message
                });
            }
        }

        // PATCH endpoint for soft-deleting (disabling) the package.
        // This endpoint sets the status of the package to INACTIVE.
        [HttpPatch(ApiEndpointConstant.ServicePackageEndpoint.DisableServicePackageEndpoint)]
        [ProducesResponseType(typeof(ServicePackageDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Soft-deleting (disabling) the package (ets the status of the package to INACTIVE).")]
        public async Task<IActionResult> DisableServicePackage([FromQuery] string id)
        {
            try
            {
                var dto = await _servicePackageService.DisableServicePackageAsync(id);
                return Ok(dto);
            }
            catch (NotFoundException nf)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Package not found",
                    Detail = nf.Message
                });
            }
            catch (DomainValidationException dv)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Cannot disable package",
                    Detail = dv.Message,
                    Extensions = { ["errorCode"] = "PackageDisableFailed" }
                });
            }
        }

        // PATCH endpoint for activating the package.
        // This endpoint sets the status of the package to ACTIVE.
        [HttpPatch(ApiEndpointConstant.ServicePackageEndpoint.ActivateServicePackageEndpoint)]
        [ProducesResponseType(typeof(ServicePackageDTO), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Activating the package (sets the status of the package to ACTIVE)")]
        public async Task<IActionResult> ActivateServicePackage([FromQuery] string id)
        {
            try
            {
                var dto = await _servicePackageService.ActivateServicePackageAsync(id);
                return Ok(dto);
            }
            catch (DomainValidationException dv)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Cannot activate package",
                    Detail = dv.Message,
                    Extensions = { ["errorCode"] = "PackageActivateFailed" }
                });
            }
            catch (NotFoundException nf)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Package not found",
                    Detail = nf.Message
                });
            }
        }
    }
}
