using CleanFoodVietAPI.Application.DTOs.ProductPriceDTOs;
using CleanFoodVietAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.ProductDTOs
{
    public record ProductDTO
    (
        Ulid ProductId,
        string ProductName,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        string Status,
        string ProductCategory,
        ProductPriceDTO price
    );
}
