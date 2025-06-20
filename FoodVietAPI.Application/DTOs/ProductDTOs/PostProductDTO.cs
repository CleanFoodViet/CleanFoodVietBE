using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.ProductDTOs
{
    public record PostProductDTO
    (
        string ProductName, 
        DateTime UpdatedAt,
        string ProductStatus, 
        string ProductCategory, 

        // Product Price Data Field
        decimal Price,
        string Currency);
}
