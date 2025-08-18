namespace CleanFoodVietAPI.Application.DTOs.ReviewDTOs
{
    public class ReviewDTO
    {
        public Ulid ReviewId { get; }
        public Ulid RetailerId { get; }
        public Ulid OrderDetailId { get; }
        public int Rating { get; }
        public string Comment { get; }
        public DateTime CreatedAt { get; }

        public ReviewDTO(
            Ulid reviewId,
            Ulid retailerId,
            Ulid orderDetailId,
            int rating,
            string comment,
            DateTime createdAt)
        {
            ReviewId = reviewId;
            RetailerId = retailerId;
            OrderDetailId = orderDetailId;
            Rating = rating;
            Comment = comment;
            CreatedAt = createdAt;
        }
    }
}
