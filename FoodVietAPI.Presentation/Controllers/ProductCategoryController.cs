using CleanFoodVietAPI.Application.DTOs.ProductCategoryDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class ProductCategoryController : BaseController<ProductCategoryController>
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(ILogger<ProductCategoryController> logger, IProductCategoryService productCategoryService)
            : base(logger)
        {
            _productCategoryService = productCategoryService;
        }

        [HttpGet(ApiEndpointConstant.ProductCategory.ProductCategoriesEndpoint)]
        [ProducesResponseType(typeof(IPaginate<GetProductCategoryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<GetProductCategoryDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary =@"Get all Gardener Product Categories:
                                      - If the fetchAll is true: return the List Format
                                      - If the fetchAll is true: return the PaginateList Format")]
        public async Task<IActionResult> GetProductCategoryList([FromQuery] int page = 1, [FromQuery] int size = 10, [FromQuery] bool fetchAll = false)
        {
            if (fetchAll)
            {
                var cateList = await _productCategoryService.GetProductCategoryList();
                return Ok(cateList);
            }

            var res = await _productCategoryService.GetProductCategoryPaginateList(page, size);
            return Ok(res);
        }

        [HttpGet(ApiEndpointConstant.ProductCategory.ProductCategoryEndpoint)]
        [ProducesResponseType(typeof(GetProductCategoryDTO), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get Gardener Product Category Information")]
        public async Task<IActionResult> GetProductCategoryInformation([FromRoute] string id)
        {
            var res = await _productCategoryService.GetProductCategoryInformation(id);
            return Ok(res);
        }

        [HttpPost(ApiEndpointConstant.ProductCategory.ProductCategoriesEndpoint)]
        [ProducesResponseType(typeof(GetProductCategoryDTO), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create Product Category")]
        public async Task<IActionResult> CreateProductCategory([FromBody] CreateProductCategoryDTO categoryData)
        {
            var res = await _productCategoryService.CreateProductCategory(categoryData);
            return StatusCode(StatusCodes.Status201Created ,res);
        }

        [HttpPatch(ApiEndpointConstant.ProductCategory.ProductCategoryEndpoint)]
        [ProducesResponseType(typeof(GetProductCategoryDTO), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Update Product Category Information")]
        public async Task<IActionResult> UpdateProductCategory([FromRoute] string id, [FromBody] UpdateProductCategoryDTO updateData)
        {
            var res = await _productCategoryService.UpdateProductCategory(id, updateData);
            return Ok(res);
        }

        [HttpDelete(ApiEndpointConstant.ProductCategory.ProductCategoryEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Delete a Product Category which do not have any Products related to")]
        public async Task<IActionResult> DeleteProductCategory([FromRoute] string id)
        {
            await _productCategoryService.DeleteProductCategory(id);
            return Ok("Delete product category successfully");
        }
    }
}
