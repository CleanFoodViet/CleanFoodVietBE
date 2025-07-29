using CleanFoodVietAPI.Application.DTOs.ProductCaertificateDTOs;
using CleanFoodVietAPI.Application.DTOs.ProductDTOs;
using CleanFoodVietAPI.Application.DTOs.ProductPriceDTOs;
using CleanFoodVietAPI.Application.DTOs.ReviewDTOs;
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
        private readonly IProductCertificateServcie _productCertificateServcie;
        public ProductController(ILogger<ProductController> logger, IProductService productService, IProductCertificateServcie productCertificateServcie) : base(logger)
        {
            _productService = productService;
            _productCertificateServcie = productCertificateServcie;
        }

        [HttpGet(ApiEndpointConstant.Product.GardenerProductsEndpoint)]
        [ProducesResponseType(typeof(IPaginate<ProductListDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ProductListDTO>), StatusCodes.Status200OK)]
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
            [FromQuery] string? category = null,
            [FromQuery] bool getAll = false)
        {
            if (getAll)
            {
                var listRes = _productService.GetGardenerAllProductList(
                    gardenerId, filterField, filterValue, sortField, sortOrder, search, category);
                return Ok(listRes);
            }

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

        [HttpPatch(ApiEndpointConstant.Product.GardenerProductEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Change a Product Status (Product Status: ACTIVE, INACTIVE (possible when no active/available post contain this product))")]
        public async Task<IActionResult> UpdateProductBasicInformation([FromRoute]string gardenerId, [FromRoute] string id, [FromBody] UpdateProductDTO request)
        {
            await _productService.UpdateProductBasicInformation(gardenerId, id, request);
            return Ok("Update product basic information successfully");
        }

        [HttpPatch(ApiEndpointConstant.Product.ProductStatusEndpoint)]
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
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create Gardener Products' Price")]
        public async Task<IActionResult> CreateProductPrice([FromRoute] string id, [FromBody] CreateProductPriceDTO request)
        {
            await _productService.CreateProductPrice(id, request);
            return Ok("Create Product Price Successfully");
        }

        [HttpPatch(ApiEndpointConstant.Product.ProductPriceEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Set product current price")]
        public async Task<IActionResult> SetProductCurrentPrice([FromRoute] string id, [FromRoute]string priceId)
        {
            await _productService.SetProductCurrentPrice(id, priceId);
            return Ok("Product price change successfully");
        }

        [HttpGet(ApiEndpointConstant.Product.ProductReviewsEndpoint)]
        [ProducesResponseType(typeof(List<ProductReviewDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Set product reviews")]
        public async Task<IActionResult> GetProductRevies([FromRoute] string id)
        {
            var res = await _productService.GetProductReviews(id);
            return Ok(res);
        }

        //Product Certificates
        [HttpGet(ApiEndpointConstant.Product.ProductCertificatesEndpoint)]
        [ProducesResponseType(typeof(List<ProductPriceDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get Gardener Products' Certificates")]
        public async Task<IActionResult> GetProductCertificateList([FromRoute] string id)
        {
            var res = await _productCertificateServcie.GetProductCertificate(id);
            return Ok(res);
        }

        [HttpPost(ApiEndpointConstant.Product.ProductCertificatesEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create Products' Certificate")]
        public async Task<IActionResult> CreateProductCertificate([FromRoute] string id, [FromBody] ProductCertificateDTO request)
        {
            await _productCertificateServcie.CreateProductCertificate(id, request);
            return Ok("Create Product Price Successfully");
        }
    }
}
