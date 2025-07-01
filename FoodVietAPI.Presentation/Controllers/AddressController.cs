using CleanFoodVietAPI.Application.DTOs.AddressDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class AddressController : BaseController<AddressController>
    {
        private readonly IAddressService _addressService;
        public AddressController(ILogger<AddressController> logger, IAddressService addressService) : base(logger)
        {
            _addressService = addressService;
        }

        [HttpGet(ApiEndpointConstant.Account.AccountAddressesEndpoint)]
        [ProducesResponseType(typeof(List<GetAddressDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all the addresses of a account")]
        public async Task<IActionResult> GetAccountAddressList([FromRoute] string id)
        {
            var res = await _addressService.GetAccountAddressList(id);
            return Ok(res);
        }

        [HttpGet(ApiEndpointConstant.Account.AccountAddressEndpoint)]
        [ProducesResponseType(typeof(GetAddressDTO), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get address detail of a account")]
        public async Task<IActionResult> GetAccountAddressDetail([FromRoute] string id, [FromRoute] string addressId)
        {
            var res = await _addressService.GetAccountAddressDetail(id, addressId);
            return Ok(res);
        }

        [HttpPost(ApiEndpointConstant.Account.AccountAddressesEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create address for a account")]
        public async Task<IActionResult> CreateAddress([FromRoute] string id, [FromBody] AddressDTO request)
        {
            await _addressService.CreateAddresss(request, id);
            return StatusCode(StatusCodes.Status201Created, "Create address successfully");
        }

        [HttpPatch(ApiEndpointConstant.Account.AccountAddressEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update address information of a account")]
        public async Task<IActionResult> UpdateAddress([FromRoute] string id, [FromRoute] string addressId, [FromBody] AddressDTO request)
        {
            await _addressService.UpdateAddress(request, addressId, id);
            return Ok("Update address successfully");
        }

        [HttpDelete(ApiEndpointConstant.Account.AccountAddressEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete address of a account")]
        public async Task<IActionResult> DeleteAddress([FromRoute] string id, [FromRoute] string addressId)
        {
            await _addressService.DeletAddress(addressId, id);
            return Ok("Delete address successfully");
        }
    }
}
