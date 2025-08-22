using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.ReviewDTOs;
using CleanFoodVietAPI.Application.Exceptions;
using CleanFoodVietAPI.Application.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.OrderEnums;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class ReviewService : BaseService<ReviewService>, IReviewService
    {
        private readonly IUnitOfWork<CleanFoodVietDbContext> _uow;

        public ReviewService(
            IUnitOfWork<CleanFoodVietDbContext> uow,
            ILogger<ReviewService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(uow, logger, mapper, httpContextAccessor)
        {
            _uow = uow;
        }

        // Creates a new review for an order detail.
        // Validates the retailer ID and order detail ID, checks if the order is completed,
        // and prevents duplicate reviews.
        // Returns the created review as a ReviewDTO.
        #region Create Review
        public async Task<ReviewDTO> CreateReviewAsync(
            Ulid retailerId,
            Ulid orderId,
            Ulid orderDetailId,
            int rating,
            string comment)
        {
            // Load OrderDetail and its parent Order
            var odRepo = _uow.GetRepository<OrderDetail>();
            var detail = await odRepo.GetAsync(
                selector: od => od,
                predicate: od => od.OrderDetailId == orderDetailId,
                include: q => q.Include(od => od.Order));

            // 1) Not found or wrong order
            if (detail == null || detail.Order.OrderId != orderId)
                throw new NotFoundException("Order detail not found for this order.");

            // 2) Wrong retailer
            if (detail.Order.RetailerId != retailerId)
                throw new DomainValidationException("Cannot review someone else’s order.");

            // 3) Only completed orders
            if (!Enum.TryParse<OrderStatusEnum>(detail.Order.Status, out var orderStatus)
                || orderStatus != OrderStatusEnum.COMPLETED)
            {
                throw new DomainValidationException(
                    "You can only review an order once it has reached the COMPLETED status.");
            }

            // Prevent duplicate
            var rvRepo = _uow.GetRepository<Review>();
            var existing = await rvRepo.GetAsync(
                selector: r => r,
                predicate: r => r.OrderDetailId == orderDetailId && r.RetailerId == retailerId);

            if (existing != null)
                throw new DomainValidationException("Review already submitted.");

            // Create and save
            var review = new Review
            {
                ReviewId = Ulid.NewUlid(),
                RetailerId = retailerId,
                OrderDetailId = orderDetailId,
                Rating = rating,
                Comment = comment ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            await rvRepo.InsertAsync(review);
            await _uow.CommitAsync();

            // Return DTO
            return new ReviewDTO(
                review.ReviewId,
                review.RetailerId,
                review.OrderDetailId,
                review.Rating,
                review.Comment,
                review.CreatedAt);
        }
        #endregion


        // Retrieves a review for a specific order detail.
        // Validates the retailer ID and order detail ID, and returns the review as a ReviewDTO.
        // If no review exists, returns null.
        #region Get Review for Order Detail
        public async Task<ReviewDTO?> GetReviewForOrderDetailAsync(string retailerId, string orderDetailId)
        {
            if (!Ulid.TryParse(retailerId, out var rid) ||
                !Ulid.TryParse(orderDetailId, out var did))
                throw new DomainValidationException("Invalid IDs.");

            var repo = _uow.GetRepository<Review>();
            var rev = await repo.GetAsync(
                selector: r => new ReviewDTO(
                    r.ReviewId,
                    r.RetailerId,
                    r.OrderDetailId,
                    r.Rating,
                    r.Comment,
                    r.CreatedAt),
                predicate: r => r.RetailerId == rid && r.OrderDetailId == did);

            return rev;
        }
        #endregion


        // Retrieves all reviews for a specific product.
        // Validates the product ID, retrieves the reviews, and returns them as a list of ProductReviewDTO.
        #region Get Product Reviews
        public async Task<List<ProductReviewDTO>> GetProductReviewsAsync(string productId)
        {
            if (!Ulid.TryParse(productId, out var pid))
                throw new DomainValidationException("Invalid product id.");

            var repo = _uow.GetRepository<Review>();

            var reviews = await repo.GetListAsync(
                include: q => q
                    .Include(r => r.OrderDetail)
                        .ThenInclude(r => r.Post)
                        .ThenInclude(r => r.Product)
                    .Include(r => r.Account),
                predicate: r => r.OrderDetail.Post.ProductId == pid,
                selector: r => new ProductReviewDTO
                {
                    ProductReviewId = r.ReviewId,
                    Name = r.Account.Name,
                    Avatar = r.Account.Avatar,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                }
            );

            // Convert the returned ICollection<T> into a List<T>
            return reviews.ToList();
        }
        #endregion

    }
}

