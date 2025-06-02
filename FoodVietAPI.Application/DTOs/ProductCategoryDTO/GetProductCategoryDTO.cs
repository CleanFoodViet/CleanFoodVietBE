using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
