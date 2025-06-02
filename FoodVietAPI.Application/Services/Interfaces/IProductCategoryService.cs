using CleanFoodVietAPI.Application.DTOs.ProductCategoryDTO;
using CleanFoodVietAPI.Data.Paginate;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IProductCategoryService
    {
        Task<IPaginate<GetProductCategoryDTO>> GetProductCategoryList(int page, int size);
        Task<GetProductCategoryDTO> GetProductCategoryInformation(string id);
        Task CreateProductCategory(CreateProductCategoryDTO category);
        Task UpdateProductCategory(string id, UpdateProductCategoryDTO data);
        Task DeleteProductCategory(string id);
    }
}
