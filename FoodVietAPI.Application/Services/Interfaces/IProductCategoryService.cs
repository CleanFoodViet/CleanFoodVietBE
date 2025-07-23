using CleanFoodVietAPI.Application.DTOs.ProductCategoryDTOs;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Exceptions;
using CleanFoodVietAPI.Data.Paginate;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IProductCategoryService
    {
        Task<IPaginate<GetProductCategoryDTO>> GetProductCategoryList(int page, int size);
        Task<GetProductCategoryDTO> GetProductCategoryInformation(string id);
        Task<GetProductCategoryDTO> CreateProductCategory(CreateProductCategoryDTO category);
        Task<GetProductCategoryDTO> UpdateProductCategory(string id, UpdateProductCategoryDTO data);
        Task DeleteProductCategory(string id);
    }
}
