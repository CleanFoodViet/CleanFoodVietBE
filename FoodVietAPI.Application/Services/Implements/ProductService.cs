using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ProductDTOs;
using CleanFoodVietAPI.Application.DTOs.ProductPriceDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Drawing;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class ProductService : BaseService<ProductService>, IProductService
    {
        public ProductService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<ProductService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<IPaginate<ProductDTO>> GetGardenerProductList(string gardenerId, int page, int size)
        {
            Ulid gardenerID = Ulid.Parse(gardenerId);
            var gardener = await _unitOfWork.GetRepository<Account>().GetAsync(predicate: g => g.AccountId == gardenerID);
            if (gardener == null) throw new BadHttpRequestException("Gardener is not found!");

            var products = await _unitOfWork.GetRepository<Product>()
                .GetPagingListAsync(predicate: p => p.GardenerId == gardenerID,
                          include: p => p.Include(x => x.ProductPrices).Include(x => x.ProductCategory),
                          selector: p => new ProductDTO(
                              p.ProductId,
                              p.ProductName,
                              p.CreatedAt,
                              p.UpdatedAt,
                              p.Status,
                              p.ProductCategory.Name,
                              _mapper.Map<ProductPriceDTO>(p.ProductPrices)),
                          page: page, size: size);

            return products;
        }

        public async Task<ProductDTO> GetProductInformation(string productId)
        {
            Ulid id = Ulid.Parse(productId);
            var products = await _unitOfWork.GetRepository<Product>()
                .GetAsync(predicate: p => p.GardenerId == id,
                          include: p => p.Include(x => x.ProductPrices).Include(x => x.ProductCategory),
                          selector: p => new ProductDTO(
                              p.ProductId,
                              p.ProductName,
                              p.CreatedAt,
                              p.UpdatedAt,
                              p.Status,
                              p.ProductCategory.Name,
                          _mapper.Map<ProductPriceDTO>(p.ProductPrices)));

            return products;
        }

        //public async Task CreateProduct(string gardenerId /*CreateProductDTO*/)
        //{

        //}

        //public async Task UpdateProduct(string productId)
        //{

        //}

        //public async Task DisableProduct(string productId)
        //{

        //}
    }
}
