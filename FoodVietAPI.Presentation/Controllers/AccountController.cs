using CleanFoodVietAPI.Application.DTOs.AccountDTO;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Application.Utils;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(typeof(IPaginate<GetAccountDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAccountList([FromQuery]int page, [FromQuery]int size)
        {
            var res = await _accountService.GetAccountList(page, size);
            return Ok(res);
        }

        [HttpGet(ApiEndpointConstant.Account.AccountEndpoint)]
        [ProducesResponseType(typeof(GetAccountDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAccountList([FromRoute] string id)
        {
            var res = await _accountService.GetAccountInformation(id);
            return Ok(res);
        }

        [HttpPatch(ApiEndpointConstant.Account.AccountEndpoint)]
        public async Task<IActionResult> GetAccountList([FromRoute] string id, [FromQuery] string status)
        {
            await _accountService.UpdateAccountStatus(id, status);
            return Ok("Update status successfully");
        }
    }
}