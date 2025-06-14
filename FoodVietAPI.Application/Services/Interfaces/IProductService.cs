using CleanFoodVietAPI.Application.DTOs.ProductDTOs;
using CleanFoodVietAPI.Data.Paginate;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<IPaginate<GetProductListDTO>> GetGardenerProductList(string gardenerId, int page, int size);
        Task<GetProductDTO> GetProductInformation(string productId);
    }
}
