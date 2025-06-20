using CleanFoodVietAPI.Application.DTOs.ProductDTOs;
using CleanFoodVietAPI.Application.DTOs.ProductPriceDTOs;
using CleanFoodVietAPI.Data.Paginate;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<IPaginate<ProductListDTO>> GetGardenerProductList(string gardenerId, int page, int size);
        Task<ProductDTO> GetProductInformation(string productId);
        Task CreateProduct(string gardenerId, CreateProductDTO createProductData);


        Task<List<ProductPriceDTO>> GetProductPrices(string productId);
    }
}
