using CleanFoodVietAPI.Application.DTOs.CartDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class CartController : BaseController<CartController>
    {
        private readonly ICartService _cartService;
        public CartController(ILogger<CartController> logger, ICartService cartService) : base(logger)
        {
            _cartService = cartService;
        }

        [HttpGet(ApiEndpointConstant.RetailerCart.RetailerCartEndpoint)]
        [ProducesResponseType(typeof(List<CartDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get a whole cart of retailer (Including many carts of Retailer which in each Cart is the List of a specific Gardeners' Products)")]
        public async Task<IActionResult> GetRetailerCart([FromRoute] string id)
        {
            var res = await _cartService.GetRetailerCarts(id);
            return Ok(res);
        }

        [HttpPut(ApiEndpointConstant.RetailerCart.RetailerCartEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Modify the whole Reatiler's Cart (Compare the After and Before Cart to Add, Update, Delete)")]
        public async Task<IActionResult> ModifyCart([FromRoute]string id, [FromBody]List<CartDTO> request)
        {
            await _cartService.ModifyCart(id, request);
            return Ok("Modify Retailers' Cart Successfully");
        }

        [HttpDelete(ApiEndpointConstant.RetailerCart.RetailerCartEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete the whole Retailer's Cart")]
        public async Task<IActionResult> DeleteCarts([FromRoute] string id)
        {
            await _cartService.DeleteCart(id);
            return Ok("Delete Retailers' Cart Successfully");
        }
    }
}
