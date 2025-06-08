using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Application.DTOs.ProductCategoryDTOs
{
    public record GetProductCategoryDTO
    (
        Ulid ProductCategoryId,
        [Required]
        string Name,
        string? Description
    );
}
