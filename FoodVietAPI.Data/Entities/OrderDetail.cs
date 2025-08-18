using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class OrderDetail
    {
        [Key]
        public Ulid OrderDetailId { get; set; }
        public Ulid OrderId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ProductUnit { get; set; } = null!;
        public string DeliveryStatus { get; set; } = null!;
        public Ulid PostId { get; set; }
        public decimal DepositAmount { get; set; }
        public int DepositPercentage { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Post Post { get; set; } = null!;
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public virtual ICollection<OrderDeliveryDetail> OrderDeliveryDetails { get; set; } = new HashSet<OrderDeliveryDetail>();
    }
}
