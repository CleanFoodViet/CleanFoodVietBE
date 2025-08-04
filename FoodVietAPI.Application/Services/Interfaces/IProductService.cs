using CleanFoodVietAPI.Application.DTOs.ProductDTOs;
using CleanFoodVietAPI.Application.DTOs.ProductPriceDTOs;
using CleanFoodVietAPI.Application.DTOs.ReviewDTOs;
using CleanFoodVietAPI.Data.Paginate;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<IPaginate<ProductListDTO>> GetGardenerProductList(
            string gardenerId, int page, int size,
            string? filterField = null,
            string? filterValue = null,
            string? sortField = null,
            string? sortOrder = "asc",
            string? search = null,
            string? category = null);

        Task<List<ProductListDTO>> GetGardenerAllProductList(string gardenerId);
        Task<ProductDTO> GetProductInformation(string productId);
        Task CreateProduct(string gardenerId, CreateProductDTO createProductData);
        Task UpdateProductBasicInformation(string gardenerId, string productId, UpdateProductDTO updateData);
        Task ChangeProductStatus(string productId, string status);

        //Product Prices
        Task<List<ProductPriceDTO>> GetProductPrices(string productId);
        Task CreateProductPrice(string productId, CreateProductPriceDTO request);
        Task SetProductCurrentPrice(string productId, string priceId);

        //Product Reviews
        Task<List<ProductReviewDTO>> GetProductReviews(string productId);
    }
}
