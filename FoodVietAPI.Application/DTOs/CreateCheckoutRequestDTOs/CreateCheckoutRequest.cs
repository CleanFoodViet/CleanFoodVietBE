using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.CreateCheckoutRequestDTOs
{
    public class CreateCheckoutRequest
    {
        public Ulid GardenerId { get; set; }
        public Ulid ServicePackageId { get; set; }
        public int Quantity { get; set; } = 1;
        public string? Location { get; set; } = "VN";
    }
}
