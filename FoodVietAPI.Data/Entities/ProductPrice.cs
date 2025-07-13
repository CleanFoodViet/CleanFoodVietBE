using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class ProductPrice
    {
        [Key]
        public Ulid ProductPriceId { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; } = null!;
        public string WeightUnit { get; set; } = null!;
        public DateTime AvailabledDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsCurrent { get; set; }
        public Ulid Productd { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
