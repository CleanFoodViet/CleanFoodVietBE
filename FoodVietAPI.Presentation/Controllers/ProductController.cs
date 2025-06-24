using CleanFoodVietAPI.Application.DTOs.ProductDTOs;
using CleanFoodVietAPI.Application.DTOs.ProductPriceDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetProductsList([FromRoute] string gardenerId, int page = 1, int size = 10)
        {
            var res = await _productService.GetGardenerProductList(gardenerId, page, size);

            return Ok(res);
        }

        [HttpGet(ApiEndpointConstant.Product.ProductEndpoint)]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsList([FromRoute] string id)
        {
            var res = await _productService.GetProductInformation(id);

            return Ok(res);
        }

        [HttpPost(ApiEndpointConstant.Product.GardenerProductsEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateProduct([FromRoute]string gardenerId, [FromBody]CreateProductDTO createProduct)
        {
            await _productService.CreateProduct(gardenerId, createProduct);
            return Ok("Create Successfully");
        }

        //Update Product

        //Disable Product

        //Get Price
        [HttpGet(ApiEndpointConstant.Product.ProductPricesEndpoint)]
        [ProducesResponseType(typeof(List<ProductPriceDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductPriceList([FromRoute]string id)
        {
            var res = await _productService.GetProductPrices(id);
            return Ok(res);
        }
        //Update Product Price
    }
}
