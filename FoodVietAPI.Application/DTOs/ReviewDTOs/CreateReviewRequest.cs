using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Application.DTOs.ReviewDTOs
{
    public class CreateReviewRequest
    {
        [Range(1, 5)] public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
