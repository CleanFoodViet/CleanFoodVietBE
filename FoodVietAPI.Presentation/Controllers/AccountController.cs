using CleanFoodVietAPI.Application.DTOs.AccountDTOs;
using CleanFoodVietAPI.Application.DTOs.AddressDTOs;
using CleanFoodVietAPI.Application.DTOs.AppointmentDTOs;
using CleanFoodVietAPI.Application.DTOs.CertificateDTOs;
using CleanFoodVietAPI.Application.DTOs.NotificationDTOs;
using CleanFoodVietAPI.Application.DTOs.PostDTOs;
using CleanFoodVietAPI.Application.Services.Implements;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class AccountController : BaseController<AccountController>
    {
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService) 
            : base(logger)
        {
            _accountService = accountService;
        }

        [HttpGet(ApiEndpointConstant.Account.RetailerAccountsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<AccountDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all the retailers' accounts")]
        public async Task<IActionResult> GetRetailerAccountList(
             [FromQuery] int page = 1,
             [FromQuery] int size = 10,
             [FromQuery] string? filterField = null,
             [FromQuery] string? filterValue = null,
             [FromQuery] string? sortField = null,
             [FromQuery] string? sortOrder = "asc",
             [FromQuery] string? search = null)
        {
            var res = await _accountService.GetRetailerAccountList(
                page, size, filterField, filterValue, sortField, sortOrder, search);
            return Ok(res);
        }

        [HttpGet(ApiEndpointConstant.Account.GardenerAccountsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<AccountDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all the gardeners' accounts")]
        public async Task<IActionResult> GetGardenerAccountList(
             [FromQuery] int page = 1,
             [FromQuery] int size = 10,
             [FromQuery] string? filterField = null,
             [FromQuery] string? filterValue = null,
             [FromQuery] string? sortField = null,
             [FromQuery] string? sortOrder = "asc",
             [FromQuery] string? search = null)
        {
            var res = await _accountService.GetGardenerAccountList(
                page, size, filterField, filterValue, sortField, sortOrder, search);
            return Ok(res);
        }

        [HttpPatch(ApiEndpointConstant.Account.AccountStatuEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update status of a account (Account Status: ACTIVE, INACTIVE, BANNED) and IsVerified (optional)")]
        public async Task<IActionResult> UpdateAccountStatus([FromRoute] string id, [FromQuery] string status, [FromQuery]bool? isVerified)
        {
            var checkVerified = isVerified == null ? false : true;

            await _accountService.UpdateAccountStatus(id, status);
            return Ok("Update status successfully");
        }

        [HttpGet(ApiEndpointConstant.Account.AccountEndpoint)]
        [ProducesResponseType(typeof(AccountDTO), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get 1 specific account's information (Include Certificate and Address List. If role does not have 1 of 2 List, the value is null)")]
        public async Task<IActionResult> GetAccountInformation([FromRoute] string id)
        {
            var res = await _accountService.GetAccountInformation(id);
            return Ok(res);
        }

        [HttpPatch(ApiEndpointConstant.Account.AccountProfileEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update an account's profile")]
        public async Task<IActionResult> UpdateProfile([FromRoute]string id, [FromBody]UpdateAccountDTO data)
        {
            await _accountService.UpdateProfile(id, data);
            return Ok("Update profile successfully");
        }

        [HttpPatch(ApiEndpointConstant.Account.AccountPasswordEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = @"Change or reset account password. action field = CHANGE/RESET")]
        public async Task<IActionResult> ChangeOrResetPassword([FromBody] ChangePasswordDto request)
        {
            await _accountService.ChangePassword(request);
            return Ok("Update account password successfully");
        }
    }
}