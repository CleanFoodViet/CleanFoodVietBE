using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Data.Entities
{
    public class OrderDeliveryDetail
    {
        [Key]
        public Ulid OrderDeliveryDetailId { get; set; }
        public Ulid OrderDeliveryId { get; set; }
        public Ulid ProductId { get; set; }
        public DateTime DeliveredAt { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductUnit { get; set; } = null!;

        public virtual OrderDelivery OrderDelivery { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
