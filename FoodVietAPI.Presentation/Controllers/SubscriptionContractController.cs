using CleanFoodVietAPI.Application.DTOs.SubscriptionContractDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class SubscriptionContractController : BaseController<SubscriptionContractController>
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionContractController(ILogger<SubscriptionContractController> logger, ISubscriptionService subscriptionService) 
            : base(logger)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet(ApiEndpointConstant.SubscriptionContract.SubscriptionContractsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<SubscriptionContractListDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListSubscriptionContractList(int page = 1, int size = 10)
        {
            var res = await _subscriptionService.GetSubscriptionContractList(page, size);

            return Ok(res);
        }

        [HttpGet(ApiEndpointConstant.SubscriptionContract.SubscriptionContractEndpoint)]
        [ProducesResponseType(typeof(SubscriptionContractDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListSubscriptionContractInformation([FromRoute]string id)
        {
            var res = await _subscriptionService.GetSubscriptionContractInformation(id);

            return Ok(res);
        }
    }
}
