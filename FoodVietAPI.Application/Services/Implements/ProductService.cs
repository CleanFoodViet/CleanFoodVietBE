using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ProductCaertificateDTOs;
using CleanFoodVietAPI.Application.DTOs.ProductDTOs;
using CleanFoodVietAPI.Application.DTOs.ProductPriceDTOs;
using CleanFoodVietAPI.Application.DTOs.ReviewDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Application.Specifications;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.OrderEnums;
using CleanFoodVietAPI.Data.Enums.PostEnums;
using CleanFoodVietAPI.Data.Enums.ProductEnums;
using CleanFoodVietAPI.Data.Exceptions;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class ProductService : BaseService<ProductService>, IProductService
    {
        public ProductService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<ProductService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<IPaginate<ProductListDTO>> GetGardenerProductList(
            string gardenerId, int page, int size,
            string? filterField = null,
            string? filterValue = null,
            string? sortField = null,
            string? sortOrder = "asc",
            string? search = null,
            string? category = null)
        {
            Ulid gardenerID = Ulid.Parse(gardenerId);
            ProductSpecification productSpecification = new ProductSpecification(filterField, filterValue, sortField, sortOrder, search);

            var gardener = await _unitOfWork.GetRepository<Account>().GetAsync(predicate: g => g.AccountId == gardenerID);
            if (gardener == null) throw new BadHttpRequestException("Gardener is not found!");

            DateTime today = DateTime.UtcNow;
            var products = await _unitOfWork.GetRepository<Product>()
                .GetPagingListAsync(
                        include: p => p.Include(x => x.ProductPrices.Where(pp => pp.IsCurrent))
                                        .Include(x => x.ProductCategory)
                                        .Include(x => x.ProductTags)
                                        .Include(x => x.ProductCertificates),
                        predicate: p => category == null ?
                                p.GardenerId == gardenerID :
                                p.GardenerId == gardenerID && p.ProductCategory.Name.ToUpper() == category.ToUpper(),
                        selector: p => new ProductListDTO(
                            p.ProductId,
                            p.ProductName,
                            p.CreatedAt,
                            p.UpdatedAt,
                            p.Status,
                            p.ProductCategory.Name,
                            p.ProductPrices.First().Price,
                            p.ProductPrices.First().Currency,
                            p.ProductPrices.First().WeightUnit,
                            p.ProductTags.Select(pt => pt.TagName).ToList(),
                            _mapper.Map<List<GetProductCertificateDTO>>(p.ProductCertificates)),
                        page: page, size: size,
                        spec: productSpecification);

            return products;
        }

        public async Task<List<ProductListDTO>> GetGardenerAllProductList(string gardenerId)
        {
            Ulid gardenerID = Ulid.Parse(gardenerId);
            var gardener = await _unitOfWork.GetRepository<Account>().GetAsync(predicate: g => g.AccountId == gardenerID);
            if (gardener == null) throw new BadHttpRequestException("Gardener is not found!");

            DateTime today = DateTime.UtcNow;
            var products = await _unitOfWork.GetRepository<Product>()
                .GetListAsync(
                        include: p => p.Include(x => x.ProductPrices.Where(pp => pp.IsCurrent))
                                        .Include(x => x.ProductCategory)
                                        .Include(x => x.ProductTags)
                                        .Include(x => x.ProductCertificates),
                        predicate: p => p.Status == ProductStatusEnum.ACTIVE.ToString() && p.GardenerId == gardenerID,
                        selector: p => new ProductListDTO(
                            p.ProductId,
                            p.ProductName,
                            p.CreatedAt,
                            p.UpdatedAt,
                            p.Status,
                            p.ProductCategory.Name,
                            p.ProductPrices.First().Price,
                            p.ProductPrices.First().Currency,
                            p.ProductPrices.First().WeightUnit,
                            p.ProductTags.Select(pt => pt.TagName).ToList(),
                            _mapper.Map<List<GetProductCertificateDTO>>(p.ProductCertificates)));

            return products.ToList();
        }

        public async Task<ProductDTO> GetProductInformation(string productId)
        {
            Ulid id = Ulid.Parse(productId);
            DateTime today = DateTime.UtcNow;

            var product = await _unitOfWork.GetRepository<Product>()
                .GetAsync(predicate: p => p.GardenerId == id,
                          include: p => p.Include(x => x.ProductPrices.Where(pp => pp.IsCurrent))
                                         .Include(x => x.ProductCategory)
                                         .Include(x => x.ProductTags),
                          selector: p => new ProductDTO(
                              p.ProductId,
                              p.ProductName,
                              p.CreatedAt,
                              p.UpdatedAt,
                              p.Status,
                              p.ProductCategory.Name,
                              p.ProductTags.Select(pt => pt.TagName).ToList(),
                              p.ProductPrices.First().ProductPriceId,
                              p.ProductPrices.First().Price,
                              p.ProductPrices.First().Currency,
                              p.ProductPrices.First().AvailabledDate,
                              p.ProductPrices.First().WeightUnit));

            if (product == null) throw new BadHttpRequestException("Product cannot be found");

            return product;
        }

        public async Task CreateProduct(string gardenerId, CreateProductDTO createProductData)
        {
            ProductCategory category = await _unitOfWork.GetRepository<ProductCategory>()
                   .GetAsync(predicate: pc => pc.ProductCategoryId == createProductData.ProductCategoryId);
            if (category == null) throw new BadHttpRequestException("Invalid input: product category is not found");

            Ulid gardenerID = Ulid.Parse(gardenerId);
            Product newProduct = _mapper.Map<Product>(createProductData);
            ProductPrice newPrice = _mapper.Map<ProductPrice>(createProductData);

            newProduct.GardenerId = gardenerID;
            newPrice.Productd = newProduct.ProductId;

            List<ProductTag> tags = _mapper.Map<List<ProductTag>>(
                createProductData.TagNames,
                 opt =>
                 {
                     opt.Items["GardenerId"] = gardenerID;
                     opt.Items["ProductId"] = newProduct.ProductId;
                 });

            List<ProductCertificate> certificates = _mapper.Map<List<ProductCertificate>>(
                    createProductData.Certificates,
                    opt => opt.Items["ProductId"] = newProduct.ProductId
                );

            await _unitOfWork.GetRepository<ProductCertificate>().InsertRangeAsync(certificates);
            await _unitOfWork.GetRepository<ProductTag>().InsertRangeAsync(tags);

            await _unitOfWork.GetRepository<Product>().InsertAsync(newProduct);
            await _unitOfWork.GetRepository<ProductPrice>().InsertAsync(newPrice);

            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occur when create product (DB query error)");
        }

        public async Task UpdateProductBasicInformation(string gardenerId, string productId, UpdateProductDTO updateData)
        {
            Ulid gardenerID = Ulid.Parse(gardenerId);
            Ulid productID = Ulid.Parse(productId);
            var product = await _unitOfWork.GetRepository<Product>()
                .GetAsync(predicate: p => p.ProductId == productID);
            if (product == null) throw new BadHttpRequestException("Product is not found");

            product.ProductName = string.IsNullOrEmpty(updateData.ProductName) ? product.ProductName : updateData.ProductName;

            var currentTags = await _unitOfWork.GetRepository<ProductTag>()
                .GetListAsync(predicate: pt => pt.ProductId == productID);

            var newTags = _mapper.Map<List<ProductTag>>(
                updateData.Tags,
                opt =>
                {
                    opt.Items["GardenerId"] = gardenerID;
                    opt.Items["ProductId"] = productID;
                });

            _unitOfWork.GetRepository<Product>().UpdateAsync(product);
            //Modify the product's tags
            _unitOfWork.GetRepository<ProductTag>().DeleteRangeAsync(currentTags);
            await _unitOfWork.GetRepository<ProductTag>().InsertRangeAsync(newTags);

            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Error occur when modify the product informations (name + tags) (DB query error)");
        }

        public async Task ChangeProductStatus(string productId, string status)
        {
            Ulid productID = Ulid.Parse(productId);
            var product = await _unitOfWork.GetRepository<Product>()
                .GetAsync(
                    predicate: p => p.ProductId == productID);
            if (product == null) throw new BadHttpRequestException("Product is not found");

            if (Enum.TryParse<ProductStatusEnum>(status.ToUpper(), out var result))
            {
                if (result == ProductStatusEnum.INACTIVE)
                {
                    var today = DateTime.UtcNow;
                    var activePost = await _unitOfWork.GetRepository<Post>()
                        .GetListAsync(predicate: po => po.ProductId == productID && (po.PostEndDate < today || po.Status != PostStatusEnum.ACTIVE.ToString()));

                    if (activePost != null && activePost?.Count() > 0) throw new UpdateRestrictedException("Cannot disable the chosen Product because it have appeared in some active/available post");

                    var inProgressOrder = await _unitOfWork.GetRepository<Order>()
                        .GetListAsync(
                            include: o => o.Include(x => x.OrderDetails),
                            predicate: o => o.OrderDetails.Any(od => od.ProductId == productID) &&
                            (o.Status != OrderStatusEnum.COMPLETED.ToString() || o.Status != OrderStatusEnum.CANCELLED.ToString())
                        );
                    if (inProgressOrder != null && inProgressOrder?.Count() > 0) throw new UpdateRestrictedException("Cannot disable the chosen Product because it have appeared in some in-progress order");
                }

                product.Status = result.ToString();
            }
            else
            {
                throw new BadHttpRequestException("Invalid Product Status input.");
            }

            product.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.GetRepository<Product>().UpdateAsync(product);

            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccess) throw new Exception("Erorr occur when change product status (DB query Error: Product not updated)");
        }

        //Product Price
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
                        pp.WeightUnit,
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

        //Product Views
        public async Task<List<ProductReviewDTO>> GetProductReviews(string productId)
        {
            Ulid productID = Ulid.Parse(productId);
            var product = await _unitOfWork.GetRepository<Product>()
                .GetAsync(predicate: p => p.ProductId == productID);
            if (product == null) throw new BadHttpRequestException($"Product is not found");

            var reviews = await _unitOfWork.GetRepository<OrderDetail>()
                .GetListAsync(
                    include: odt => odt.Include(x => x.Reviews).ThenInclude(x => x.Account),
                    predicate: odt => odt.ProductId == productID,
                    selector: odt => odt.Reviews
                );

            var reviewDTOs = reviews
                .SelectMany(r => r)
                .Select(r => new ProductReviewDTO
                {
                    Name = r.Account.Name,
                    Avatar = r.Account.Avatar,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                })
                .ToList();

            return reviewDTOs;
        }
    }
}
