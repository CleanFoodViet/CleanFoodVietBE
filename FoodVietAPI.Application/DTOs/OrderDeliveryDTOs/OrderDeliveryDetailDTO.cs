using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.OrderDeliveryDTOs
{
    public record OrderDeliveryDetailDTO
    {
        public Ulid OrderDeliveryDetailId { get; set; }
        public Ulid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public DateTime DeliveredAt { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductUnit { get; set; } = null!;
    }
}
