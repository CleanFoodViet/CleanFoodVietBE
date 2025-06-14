using CleanFoodVietAPI.Application.DTOs.ProductDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class ProductController : BaseController<ProductController>
    {
        private readonly IProductService _productService;
        public ProductController(ILogger<ProductController> logger, IProductService productService) : base(logger)
        {
            _productService = productService;
        }

        [HttpGet(ApiEndpointConstant.Product.GardenerProductsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<GetProductListDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsList([FromRoute] string gardenerId, int page = 1, int size = 10)
        {
            var res = await _productService.GetGardenerProductList(gardenerId, page, size);

            return Ok(res);
        }

        [HttpGet(ApiEndpointConstant.Product.ProductEndpoint)]
        [ProducesResponseType(typeof(GetProductDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsList([FromRoute] string id)
        {
            var res = await _productService.GetProductInformation(id);

            return Ok(res);
        }
    }
}
