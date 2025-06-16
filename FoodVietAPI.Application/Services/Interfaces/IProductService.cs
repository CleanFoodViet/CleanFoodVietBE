using CleanFoodVietAPI.Application.DTOs.ProductDTOs;
using CleanFoodVietAPI.Data.Paginate;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<IPaginate<ProductListDTO>> GetGardenerProductList(string gardenerId, int page, int size);
        Task<ProductDTO> GetProductInformation(string productId);
    }
}
