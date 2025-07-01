using CleanFoodVietAPI.Application.DTOs.ProductCategoryDTOs;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Exceptions;
using CleanFoodVietAPI.Data.Paginate;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IProductCategoryService
    {
        Task<IPaginate<GetProductCategoryDTO>> GetProductCategoryList(int page, int size, string gardenerId);
        Task<GetProductCategoryDTO> GetProductCategoryInformation(string gardenerId, string id);
        Task<GetProductCategoryDTO> CreateProductCategory(CreateProductCategoryDTO category, string gardenerId);
        Task<GetProductCategoryDTO> UpdateProductCategory(string id, string gardenerId, UpdateProductCategoryDTO data);
        Task DeleteProductCategory(string id, string gardenerId);
    }
}
