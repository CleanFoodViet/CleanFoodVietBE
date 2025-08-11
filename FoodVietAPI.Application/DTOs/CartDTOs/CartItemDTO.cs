using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Application.DTOs.CartDTOs
{
    public record CartItemDTO
    {
        public Ulid? CartItemId { get; set; }
        public Ulid? CartId { get; set; }
        public Ulid ProductId { get; set; }
        public string ProductName { get; set; } = null!;

        [Range(0, double.MaxValue, ErrorMessage = "Price must be 0 or greater.")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be 1 or greater.")]
        public int Quantity { get; set; }
        public string ProductUnit { get; set; } = null!;
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
    }
}
