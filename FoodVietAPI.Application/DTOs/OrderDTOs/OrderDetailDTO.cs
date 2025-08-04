namespace CleanFoodVietAPI.Application.DTOs.OrderDTOs
{
    public record OrderDetailDTO
    {
        public Ulid OrderDetailId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ProductUnit { get; set; } = null!;
        public string DeliveryStatus { get; set; } = null!;
        public Ulid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string WeightUnit { get; set; } = null!;
        public string Currency { get; set; } = null!;
        public int DeliveredQuantity { get; set; }
    }
}
