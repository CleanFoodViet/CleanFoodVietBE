using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ProductCategoryDTO;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Implements
{

    public class ProductCategoryService : BaseService<ProductCategoryService>, IProductCategoryService
    {
        public ProductCategoryService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<ProductCategoryService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<IPaginate<GetProductCategoryDTO>> GetProductCategoryList(int page, int size)
        {
            var categories = await _unitOfWork.GetRepository<ProductCategory>()
                .GetPagingListAsync(
                selector: pc => new GetProductCategoryDTO(
                    pc.ProductCategoryId,
                    pc.Name,
                    pc.Description),
                page: page, size: size);

            return categories;
        }

        public async Task<GetProductCategoryDTO> GetProductCategoryInformation(string id)
        {
            Ulid categoryId = Ulid.Parse(id);
            var category = await _unitOfWork.GetRepository<ProductCategory>()
                .GetAsync(predicate: pc => pc.ProductCategoryId == categoryId,
                          selector: pc => new GetProductCategoryDTO(
                                            pc.ProductCategoryId,
                                            pc.Name,
                                            pc.Description));

            if (category == null) throw new BadHttpRequestException("Product category is not found");

            return category;
        }

        public async Task<GetProductCategoryDTO> CreateProductCategory(CreateProductCategoryDTO category)
        {
            ProductCategory newCategory = _mapper.Map<ProductCategory>(category);
            await _unitOfWork.GetRepository<ProductCategory>().InsertAsync(newCategory);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occur when insert new product category");
            
            var productCategoryDTO = _mapper.Map<GetProductCategoryDTO>(newCategory);
            return productCategoryDTO;
        }

        public async Task<GetProductCategoryDTO> UpdateProductCategory(string id, UpdateProductCategoryDTO data)
        {
            Ulid categoryId = Ulid.Parse(id);
            var category = await _unitOfWork.GetRepository<ProductCategory>()
                .GetAsync(predicate: pc => pc.ProductCategoryId == categoryId);

            if (category == null) throw new BadHttpRequestException("Product category is not found");

            _mapper.Map(data, category);

            _unitOfWork.GetRepository<ProductCategory>().UpdateAsync(category);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occur when updating product category");

            var productCategoryDTO = _mapper.Map<GetProductCategoryDTO>(category);
            return productCategoryDTO;
        }

        public async Task DeleteProductCategory(string id)
        {
            Ulid categoryId = Ulid.Parse(id);
            var category = await _unitOfWork.GetRepository<ProductCategory>()
                .GetAsync(predicate: pc => pc.ProductCategoryId == categoryId);
            if (category == null) throw new BadHttpRequestException("Product category is not found");

            _unitOfWork.GetRepository<ProductCategory>().DeleteAsync(category);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occur when deleting product category");
        }
    }
}
