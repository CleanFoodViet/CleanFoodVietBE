using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.OrderDeliveryDTOs
{
    public record OrderDeliveryDTO
    {
        public Ulid OrderDeliveryId { get; set; }
        public Ulid OrderId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryStatus { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? Note { get; set; }
        public List<OrderDeliveryDetailDTO>? OrderDeliveryDetails { get; set; }
    }
}
