using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ProductDTOs;
using CleanFoodVietAPI.Application.DTOs.ProductPriceDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.PostEnums;
using CleanFoodVietAPI.Data.Enums.ProductEnums;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class ProductService : BaseService<ProductService>, IProductService
    {
        public ProductService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<ProductService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<IPaginate<ProductListDTO>> GetGardenerProductList(string gardenerId, int page, int size)
        {
            Ulid gardenerID = Ulid.Parse(gardenerId);
            var gardener = await _unitOfWork.GetRepository<Account>().GetAsync(predicate: g => g.AccountId == gardenerID);
            if (gardener == null) throw new BadHttpRequestException("Gardener is not found!");

            DateTime today = DateTime.UtcNow;
            var products = await _unitOfWork.GetRepository<Product>()
                .GetPagingListAsync(predicate: p => p.GardenerId == gardenerID,
                          include: p => p.Include(x => x.ProductPrices.Where(pp => pp.IsCurrent))
                                         .Include(x => x.ProductCategory),
                          selector: p => new ProductListDTO(
                              p.ProductId,
                              p.ProductName,
                              p.UpdatedAt,
                              p.Status,
                              p.ProductCategory.Name,
                              p.ProductPrices.First().Price,
                              p.ProductPrices.First().Currency),
                          page: page, size: size);

            return products;
        }

        public async Task<ProductDTO> GetProductInformation(string productId)
        {
            Ulid id = Ulid.Parse(productId);
            DateTime today = DateTime.UtcNow;

            var product = await _unitOfWork.GetRepository<Product>()
                .GetAsync(predicate: p => p.GardenerId == id,
                          include: p => p.Include(x => x.ProductPrices.Where(pp => pp.IsCurrent))
                                         .Include(x => x.ProductCategory),
                          selector: p => new ProductDTO(
                              p.ProductId,
                              p.ProductName,
                              p.CreatedAt,
                              p.UpdatedAt,
                              p.Status,
                              p.ProductCategory.Name,
                              p.ProductPrices.First().ProductPriceId,
                              p.ProductPrices.First().Price,
                              p.ProductPrices.First().Currency,
                              p.ProductPrices.First().AvailabledDate));

            if (product == null) throw new BadHttpRequestException("Product cannot be found");

            return product;
        }

        public async Task CreateProduct(string gardenerId, CreateProductDTO createProductData)
        {
            Ulid gardenerID = Ulid.Parse(gardenerId);
            Product newProduct = _mapper.Map<Product>(createProductData);
            ProductPrice newPrice = _mapper.Map<ProductPrice>(createProductData);

            newProduct.GardenerId = gardenerID;
            newPrice.Productd = newProduct.ProductId;

            ProductCategory category = await _unitOfWork.GetRepository<ProductCategory>()
                   .GetAsync(predicate: pc => pc.ProductCategoryId == createProductData.ProductCategoryId);

            if (category == null)
            {
                category = _mapper.Map<ProductCategory>(createProductData);
                category.GardenerId = gardenerID;
                newProduct.ProductCategoryId = category.ProductCategoryId;

                await _unitOfWork.GetRepository<ProductCategory>().InsertAsync(category);
            }

            await _unitOfWork.GetRepository<Product>().InsertAsync(newProduct);
            await _unitOfWork.GetRepository<ProductPrice>().InsertAsync(newPrice);

            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occur when create product (DB query error)");
        }

        public async Task ChangeProductStatus(string productId, string status)
        {
            Ulid productID = Ulid.Parse(productId);
            var product = await _unitOfWork.GetRepository<Product>()
                .GetAsync(predicate: p => p.ProductId == productID);
            if (product == null) throw new BadHttpRequestException("Product is not found");

            if (Enum.TryParse<ProductStatusEnum>(status.ToUpper(), out var result))
            {
                if (result == ProductStatusEnum.INACTIVE)
                {
                    var today = DateTime.UtcNow;
                    var activePost = await _unitOfWork.GetRepository<Post>()
                        .GetListAsync(predicate: po => po.ProductId == productID && (po.PostEndDate < today || po.Status != PostStatusEnum.ACTIVE.ToString()));

                    if (activePost != null) throw new BadHttpRequestException("Cannot disable the chosen Product because it have appeared in some active/available post");
                }

                product.Status = result.ToString();
            }
            else
            {
                throw new BadHttpRequestException("Invalid Product Status input.");
            }

            product.UpdatedAt = DateTime.UtcNow;

            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Erorr occur when change product status (DB query Error)");
        }

        public async Task<List<ProductPriceDTO>> GetProductPrices(string productId)
        {
            Ulid productID = Ulid.Parse(productId);
            var priceList = await _unitOfWork.GetRepository<ProductPrice>()
                .GetListAsync(
                    predicate: pp => pp.Productd == productID,
                    selector: pp => new ProductPriceDTO(
                        pp.ProductPriceId,
                        pp.Price,
                        pp.Currency,
                        pp.AvailabledDate,
                        pp.CreatedAt,
                        pp.UpdatedAt,
                        pp.IsCurrent)
                );

            return priceList.ToList();
        }

        public async Task CreateProductPrice(string productId, CreateProductPriceDTO request)
        {
            Ulid productID = Ulid.Parse(productId);
            var product = await _unitOfWork.GetRepository<Product>()
                .GetAsync(predicate: p => p.ProductId == productID);
            if (product == null) throw new BadHttpRequestException("Product is not found");

            var newProduct = _mapper.Map<ProductPrice>(request);
            newProduct.Productd = productID;

            await _unitOfWork.GetRepository<ProductPrice>().InsertAsync(newProduct);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Erorr occur when create product price (DB query Error)");
        }

        public async Task SetProductCurrentPrice(string productId, string priceId)
        {
            Ulid productID = Ulid.Parse(productId);
            var product = await _unitOfWork.GetRepository<Product>()
                .GetAsync(predicate: p => p.ProductId == productID);
            if (product == null) throw new BadHttpRequestException("Product is not found");

            var currentPrice = await _unitOfWork.GetRepository<ProductPrice>()
                .GetAsync(predicate: pp => pp.Productd == productID && pp.IsCurrent);

            Ulid priceID = Ulid.Parse(priceId);
            var updatedPrice = await _unitOfWork.GetRepository<ProductPrice>()
                .GetAsync(predicate: pp => pp.ProductPriceId == priceID);
            if (updatedPrice == null) throw new BadHttpRequestException("Product price is not found");

            updatedPrice.IsCurrent = true;
            updatedPrice.UpdatedAt = DateTime.UtcNow;
            currentPrice.IsCurrent = false;
            currentPrice.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.GetRepository<ProductPrice>().UpdateAsync(currentPrice);
            _unitOfWork.GetRepository<ProductPrice>().UpdateAsync(updatedPrice);

            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Erorr occur when change product price (DB query Error)");
        }
    }
}
