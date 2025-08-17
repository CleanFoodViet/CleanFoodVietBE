using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class Order
    {
        [Key]
        public Ulid OrderId { get; set; }
        public Ulid RetailerId { get; set; }
        public Ulid GardenerId { get; set; }
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public decimal ShippingCost { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string? CancelReason { get; set; }
        public string ShippingAddress { get; set; } = null!;

        public decimal TotalDepositAmount { get; set; }

        public virtual Account Retailer { get; set; } = null!;
        public virtual Account Gardener { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();
        public virtual ICollection<OrderDelivery> OrderDeliveries { get; set; } = new HashSet<OrderDelivery>();
    }
}
