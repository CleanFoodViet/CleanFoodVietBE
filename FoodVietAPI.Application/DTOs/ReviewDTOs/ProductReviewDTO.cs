namespace CleanFoodVietAPI.Application.DTOs.ReviewDTOs
{
    public record ProductReviewDTO
    {
        public Ulid ProductReviewId { get; set; }
        public string Name { get; set; } = null!;
        public string Avatar { get; set; } = null!;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
