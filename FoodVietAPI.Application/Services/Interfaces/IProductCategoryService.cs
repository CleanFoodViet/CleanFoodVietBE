using CleanFoodVietAPI.Application.DTOs.ProductCategoryDTOs;
using CleanFoodVietAPI.Data.Paginate;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IProductCategoryService
    {
        Task<IPaginate<GetProductCategoryDTO>> GetProductCategoryPaginateList(int page, int size);
        Task<List<GetProductCategoryDTO>> GetProductCategoryList();
        Task<GetProductCategoryDTO> GetProductCategoryInformation(string id);
        Task<GetProductCategoryDTO> CreateProductCategory(CreateProductCategoryDTO category);
        Task<GetProductCategoryDTO> UpdateProductCategory(string id, UpdateProductCategoryDTO data);
        Task DeleteProductCategory(string id);
    }
}
