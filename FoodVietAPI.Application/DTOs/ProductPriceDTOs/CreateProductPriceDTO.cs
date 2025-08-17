using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Application.DTOs.ProductPriceDTOs
{
    public record CreateProductPriceDTO
    {
        //public Ulid ProductPriceId { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Price must be 0 or greater.")]
        public decimal Price { get; set; }
        public string Currency { get; set; } = null!;
        public string WeightUnit { get; set; } = null!;
        public DateTime AvailabledDate { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
        public bool IsCurrent { get; set; }
        //public Ulid Productd { get; set; }
    }
}
