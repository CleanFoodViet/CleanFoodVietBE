using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.ProductDTOs
{
    public record GetProductListDTO
    (
        Ulid ProductId,
        string ProductName,
        DateTime UpdatedAt,
        string Status,
        string ProductCategory,
        decimal Price,
        string Currency
    );
}
