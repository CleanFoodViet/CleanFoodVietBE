using CleanFoodVietAPI.Application.DTOs.ProductDTOs;
using CleanFoodVietAPI.Application.DTOs.ProductPriceDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Drawing;
using ZstdSharp.Unsafe;

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
        [ProducesResponseType(typeof(IPaginate<ProductListDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get all Gardener Products")]
        public async Task<IActionResult> GetProductsList(
            [FromRoute] string gardenerId,
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] string? filterField = null,
            [FromQuery] string? filterValue = null,
            [FromQuery] string? sortField = null,
            [FromQuery] string? sortOrder = "asc",
            [FromQuery] string? search = null,
            [FromQuery] string? category = null)
        {
            var res = await _productService.GetGardenerProductList(
                gardenerId, page, size, filterField, filterValue, sortField, sortOrder, search, category);

            return Ok(res);
        }

        [HttpGet(ApiEndpointConstant.Product.ProductEndpoint)]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get Gardener Product Information")]
        public async Task<IActionResult> GetProductsList([FromRoute] string id)
        {
            var res = await _productService.GetProductInformation(id);

            return Ok(res);
        }

        [HttpPost(ApiEndpointConstant.Product.GardenerProductsEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create a Product")]
        public async Task<IActionResult> CreateProduct([FromRoute]string gardenerId, [FromBody]CreateProductDTO createProduct)
        {
            await _productService.CreateProduct(gardenerId, createProduct);
            return Ok("Create Successfully");
        }

        [HttpPatch(ApiEndpointConstant.Product.ProductEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Change a Product Status (Product Status: ACTIVE, INACTIVE (possible when no active/available post contain this product))")]
        public async Task<IActionResult> ChangeProductStatus([FromRoute] string id, [FromQuery] string status)
        {
            await _productService.ChangeProductStatus(id, status);
            return Ok("Change product status successfully");
        }

        //Get Price
        [HttpGet(ApiEndpointConstant.Product.ProductPricesEndpoint)]
        [ProducesResponseType(typeof(List<ProductPriceDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get Gardener Products' Price")]
        public async Task<IActionResult> GetProductPriceList([FromRoute]string id)
        {
            var res = await _productService.GetProductPrices(id);
            return Ok(res);
        }

        [HttpPost(ApiEndpointConstant.Product.ProductPricesEndpoint)]
        [ProducesResponseType(typeof(List<ProductPriceDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create Gardener Products' Price")]
        public async Task<IActionResult> CreateProductPrice([FromRoute] string id, [FromQuery] CreateProductPriceDTO request)
        {
            await _productService.CreateProductPrice(id, request);
            return Ok("Create Product Price Successfully");
        }

        [HttpPatch(ApiEndpointConstant.Product.ProductReviewsEndpoint)]
        [ProducesResponseType(typeof(List<ProductPriceDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Set product current price")]
        public async Task<IActionResult> SetProductCurrentPrice([FromRoute] string id, [FromRoute]string priceId)
        {
            await _productService.SetProductCurrentPrice(id, priceId);
            return Ok("Product price change successfully");
        }
    }
}
