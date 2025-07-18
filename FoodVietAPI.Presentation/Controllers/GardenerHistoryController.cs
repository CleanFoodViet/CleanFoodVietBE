using CleanFoodVietAPI.Application.DTOs.PaymentHistoryDTOs;
using CleanFoodVietAPI.Application.DTOs.SubscriptionContractDTOs;
using CleanFoodVietAPI.Application.Exceptions;
using CleanFoodVietAPI.Application.Services.Implements;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using static CleanFoodVietAPI.Presentation.Constants.ApiEndpointConstant;


namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class GardenerHistoryController : BaseController<GardenerHistoryController>
    {
        private readonly IServicePackageOrderService _orderSvc;
        private readonly ISubscriptionContractService _contractSvc;

        public GardenerHistoryController(
            ILogger<GardenerHistoryController> logger,
            IServicePackageOrderService orderSvc,
            ISubscriptionContractService contractSvc)
            : base(logger)
        {
            _orderSvc = orderSvc;
            _contractSvc = contractSvc;
        }

        // GET /api/v1/gardeners/payments-history?gardenerId={id}
        [HttpGet(GardenerEndpoint.PaymentsHistoryEndpoint)]
        [ProducesResponseType(typeof(IReadOnlyList<PaymentHistoryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPaymentHistory([FromQuery, Required] string gardenerId, [FromQuery]int page = 1, [FromQuery]int size = 10)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var list = await _orderSvc.GetPaymentHistoryAsync(gardenerId, page, size);
                return Ok(list);
            }
            catch (DomainValidationException dv)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Gardener ID",
                    Detail = dv.Message,
                    Extensions = { ["errorCode"] = "InvalidGardenerId" }
                });
            }
        }

        // GET /api/v1/gardeners/contracts-history?gardenerId={id}
        [HttpGet(GardenerEndpoint.ContractsHistoryEndpoint)]
        [ProducesResponseType(typeof(IReadOnlyList<ContractHistoryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetContractHistory([FromQuery, Required] string gardenerId, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var list = await _contractSvc.GetContractHistoryAsync(gardenerId, page, size);
                return Ok(list);
            }
            catch (DomainValidationException dv)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Gardener ID",
                    Detail = dv.Message,
                    Extensions = { ["errorCode"] = "InvalidGardenerId" }
                });
            }
        }
    }
}
