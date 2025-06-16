using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Application.DTOs.ProductCategoryDTOs
{
    public record UpdateProductCategoryDTO
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
