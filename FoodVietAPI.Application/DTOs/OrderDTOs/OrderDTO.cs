using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.OrderDTOs
{
    public record OrderDTO
    {
        public Ulid OrderId { get; set; }
        public Ulid RetailerId { get; set; }
        public Ulid GardenerId { get; set; }
        public string AccountName { get; set; } = null!;
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public List<OrderDetailDTO> orderDetails { get; set; } = null!;
    }
}
