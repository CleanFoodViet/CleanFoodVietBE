using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class Review
    {
        [Key]
        public Ulid ReviewId { get; set; }
        public Ulid RetailerId { get; set; }
        public Ulid OrderItemId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual OrderItem OrderItem { get; set; } = null!;
    }
}
