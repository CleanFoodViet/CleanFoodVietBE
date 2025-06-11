using CleanFoodVietAPI.Application.DTOs.AccountDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class AccountController : BaseController<AccountController>
    {
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService) : base(logger)
        {
            _accountService = accountService;
        }

        [HttpGet(ApiEndpointConstant.Account.AccountsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<AccountDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAccountList([FromQuery]int page = 1, [FromQuery]int size = 10)
        {
            var res = await _accountService.GetAccountList(page, size);
            return Ok(res);
        }

        [HttpPatch(ApiEndpointConstant.Account.AccountEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAccountStatus([FromRoute] string id, [FromQuery] string status)
        {
            await _accountService.UpdateAccountStatus(id, status);
            return Ok("Update status successfully");
        }

        [HttpGet(ApiEndpointConstant.Account.AccountEndpoint)]
        [ProducesResponseType(typeof(AccountDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAccountProfile([FromRoute] string id, [FromQuery] string? role = null)
        {
            var res = await _accountService.GetAccountInformation(id);
            return Ok(res);
        }

        [HttpPatch(ApiEndpointConstant.Account.AccountProfileEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProfile([FromRoute]string id, [FromBody]UpdateAccountDTO data)
        {
            await _accountService.UpdateProfile(id, data);
            return Ok("Update profile successfully");
        }
    }
}