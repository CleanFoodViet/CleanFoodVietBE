using CleanFoodVietAPI.Application.DTOs.ReviewDTOs;

namespace CleanFoodVietAPI.Application.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewDTO> CreateReviewAsync(CreateReviewRequest req);

        Task<ReviewDTO?> GetReviewForOrderDetailAsync(string retailerId, string orderDetailId);

        // Switched from IReadOnlyList<> to List<>
        Task<List<ProductReviewDTO>> GetProductReviewsAsync(string productId);
    }
}
