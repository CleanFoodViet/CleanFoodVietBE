using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class CartItem
    {
        [Key]
        public Ulid CartItemId { get; set; }
        public Ulid CartId { get; set; }
        public Ulid ProductId { get; set; }
        public Ulid PostId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ProductUnit { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public decimal DepositAmount { get; set; }
        public int DepositPercentage { get; set; }

        public virtual Cart Cart { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
