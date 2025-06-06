using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class OrderItem
    {
        [Key]
        public Ulid OrderItemId { get; set; }
        public Ulid OrderId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ProductUnit { get; set; } = null!;
        public string ShippingStatus { get; set; } = null!;
        public Ulid ProductId { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
    }
}
