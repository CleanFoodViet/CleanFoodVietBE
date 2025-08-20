using CleanFoodVietAPI.Application.DTOs.ReviewDTOs;

namespace CleanFoodVietAPI.Application.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewDTO> CreateReviewAsync(
            Ulid retailerId,
            Ulid orderId,
            Ulid orderDetailId,
            int rating,
            string comment);

        Task<ReviewDTO?> GetReviewForOrderDetailAsync(string retailerId, string orderDetailId);

        // Switched from IReadOnlyList<> to List<>
        Task<List<ProductReviewDTO>> GetProductReviewsAsync(string productId);
    }
}
