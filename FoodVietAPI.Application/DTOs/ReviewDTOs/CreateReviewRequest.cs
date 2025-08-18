using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Application.DTOs.ReviewDTOs
{
    public class CreateReviewRequest
    {
        [Required] public string RetailerId { get; set; } = null!;
        [Required] public string OrderDetailId { get; set; } = null!;
        [Range(1, 5)] public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
