using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.ProductDTOs
{
    public class UpdateProductDTO
    {
        public string? ProductName { get; set; }
        public List<string>? Tags { get; set; }
    }
}
