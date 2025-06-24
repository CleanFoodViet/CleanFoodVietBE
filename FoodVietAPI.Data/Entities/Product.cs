using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class Product
    {
        [Key]
        public Ulid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Status { get; set; } = null!;
        public Ulid ProductCategoryId { get; set; }
        public Ulid GardenerId { get; set; }

        public virtual ProductCategory ProductCategory { get; set; } = null!;
        public virtual Account Gardener { get; set; } = null!;
        public virtual ICollection<ProductPrice> ProductPrices { get; set; } = new HashSet<ProductPrice>();
        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
        public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new HashSet<OrderDetail>();
        public virtual ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();
        public virtual ICollection<OrderDeliveryDetail> OrderDeliveryDetails { get; set; } = new HashSet<OrderDeliveryDetail>();
    }
}
