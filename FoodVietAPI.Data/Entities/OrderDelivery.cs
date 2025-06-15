using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Data.Entities
{
    public class OrderDelivery
    {
        [Key]
        public Ulid OrderDeliveryId { get; set; }
        public Ulid OrderId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryStatus { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual ICollection<OrderDeliveryItem> OrderDeliveryItems { get; set; } = new HashSet<OrderDeliveryItem>();
    }
}
