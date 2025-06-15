using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Data.Entities
{
    public class OrderDeliveryItem
    {
        [Key]
        public Ulid OrderDeliveryItemId { get; set; }
        public Ulid OrderDeliveryId { get; set; }
        public Ulid OrderItemId { get; set; }
        public int Quantity { get; set; }
        public DateTime DeliveredAt { get; set; }

        public virtual OrderDelivery OrderDelivery { get; set; } = null!;
        public virtual OrderItem OrderItem { get; set; } = null!;
    }
}
