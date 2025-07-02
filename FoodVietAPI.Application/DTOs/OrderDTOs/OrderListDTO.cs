using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.OrderDTOs
{
    public record OrderListDTO
    (
        Ulid OrderId,
        Ulid RetailerId,
        Ulid GardenerId,
        string Status,
        decimal TotalAmount,
        DateTime CreatedAt,
        int ProductTypeAmount
    );
}
