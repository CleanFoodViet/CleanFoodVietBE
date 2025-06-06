using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Application.DTOs.ProductCategoryDTO
{
    public record GetProductCategoryDTO
    (
        Ulid ProductCategoryId,
        [Required]
        string Name,
        string? Description
    );
}
