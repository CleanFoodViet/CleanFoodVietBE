using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Application.DTOs.ProductCategoryDTO
{
    public record CreateProductCategoryDTO
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
