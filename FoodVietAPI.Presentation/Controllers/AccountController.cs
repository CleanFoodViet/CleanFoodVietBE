using CleanFoodVietAPI.Application.DTOs.AccountDTOs;
using CleanFoodVietAPI.Application.DTOs.AddressDTOs;
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
        private readonly IAddressService _addressService;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService, IAddressService addressService) 
            : base(logger)
        {
            _accountService = accountService;
            _addressService = addressService;
        }

        [HttpGet(ApiEndpointConstant.Account.RetailerAccountsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<AccountDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRetailerAccountList([FromQuery]int page = 1, [FromQuery]int size = 10)
        {
            var res = await _accountService.GetRetailerAccountList(page, size);
            return Ok(res);
        }

        [HttpGet(ApiEndpointConstant.Account.GardenerAccountsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<AccountDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGardenerAccountList([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var res = await _accountService.GetGardenerAccountList(page, size);
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
        public async Task<IActionResult> GetAccountInformation([FromRoute] string id)
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

        //Account address controller
        [HttpGet(ApiEndpointConstant.Account.AccountAddressesEndpoint)]
        [ProducesResponseType(typeof(List<GetAddressDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAccountAddressList([FromRoute]string id)
        {
            var res = await _addressService.GetAccountAddressList(id);
            return Ok(res);
        }

        [HttpGet(ApiEndpointConstant.Account.AccountAddressEndpoint)]
        [ProducesResponseType(typeof(GetAddressDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAccountAddressDetail([FromRoute]string id, [FromRoute]string addressId)
        {
            var res = await _addressService.GetAccountAddressDetail(id, addressId);
            return Ok(res);
        }

        [HttpPost(ApiEndpointConstant.Account.AccountAddressesEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAddres([FromRoute]string id, [FromBody]AddressDTO request)
        {
            await _addressService.CreateAddresss(request, id);
            return StatusCode(StatusCodes.Status201Created, "Create address successfully");
        }

        [HttpPatch(ApiEndpointConstant.Account.AccountAddressEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAddress([FromRoute] string id, [FromRoute] string addressId, [FromBody]AddressDTO request)
        {
            await _addressService.UpdateAddress(request, addressId, id);
            return Ok("Update address successfully");
        }

        [HttpDelete(ApiEndpointConstant.Account.AccountAddressEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAddress([FromRoute] string id, [FromRoute] string addressId)
        {
            await _addressService.DeletAddress(addressId, id);
            return Ok("Delete address successfully");
        }
    }
}