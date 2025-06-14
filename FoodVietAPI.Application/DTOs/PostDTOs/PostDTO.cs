using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.PostDTOs
{
    public record PostDTO
    (
        //Post Data Field
         Ulid PostId,
         Ulid GardenerId,
         string Title, 
         string Content, 
         DateTime HarvestDate,
         string PostStatus,
         decimal Rating,
         DateTime CreatedAt,
         DateTime PostEndDate,
         int Priority,

    //Product Data Field
        Ulid ProductId,
        string ProductName,
        DateTime UpdatedAt,
        string ProductStatus,
        string ProductCategory,

        //Product Price Data Field
        decimal Price,
        string Currency,
        DateTime AvailabledDate
    );
}
