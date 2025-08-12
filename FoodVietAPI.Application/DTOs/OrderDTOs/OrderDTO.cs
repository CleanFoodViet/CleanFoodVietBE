namespace CleanFoodVietAPI.Application.DTOs.OrderDTOs
{
    public record OrderDTO
    {
        public Ulid OrderId { get; set; }
        public Ulid RetailerId { get; set; }
        public Ulid GardenerId { get; set; }
        public string AccountName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public decimal ShippingCost { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string? CancelReason { get; set; }
        public string ShippingAddress { get; set; } = null!;
        public List<OrderDetailDTO> orderDetails { get; set; } = null!;
    }
}
