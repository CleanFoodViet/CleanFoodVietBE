using System.ComponentModel.DataAnnotations;

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
        public string? Note { get; set; } 
        public decimal TotalAmount { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual ICollection<OrderDeliveryDetail> OrderDeliveryDetails { get; set; } = new HashSet<OrderDeliveryDetail>();
    }
}
