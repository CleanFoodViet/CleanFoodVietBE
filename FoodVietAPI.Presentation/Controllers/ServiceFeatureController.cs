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
    [Route(ApiEndpointConstant.ServiceFeature.ServiceFeaturesEndpoint)]
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

        [HttpGet]
        [ProducesResponseType(typeof(IPaginate<ServiceFeatureDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetServiceFeatureList([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var features = await _serviceFeatureService.GetServiceFeatureList(page, size);
            return Ok(features);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ServiceFeatureDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateServiceFeature([FromBody] CreateServiceFeatureDTO createDto)
        {
            var createdFeature = await _serviceFeatureService.CreateServiceFeature(createDto);
            return StatusCode(StatusCodes.Status201Created, createdFeature);
        }
    }
}
