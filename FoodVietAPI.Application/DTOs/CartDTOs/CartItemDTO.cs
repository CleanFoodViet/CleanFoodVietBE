namespace CleanFoodVietAPI.Application.DTOs.CartDTOs
{
    public record CartItemDTO
    {
        public Ulid? CartItemId { get; set; }
        public Ulid? CartId { get; set; }
        public Ulid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ProductUnit { get; set; } = null!;
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
    }
}
