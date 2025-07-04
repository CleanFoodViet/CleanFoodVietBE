namespace CleanFoodVietAPI.Application.DTOs.OrderDeliveryDTOs
{
    public record CreateOrderDelieryDTO
    {
        //public Ulid OrderDeliveryId { get; set; }
        public Ulid OrderId { get; set; }
        public DateTime DeliveryDate { get; set; }
        //public string DeliveryStatus { get; set; } = null!;
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
        public string? Note { get; set; }

        public List<CreateOrderDelieryDetailsDTO> delieryDetailsDTOs { get; set; } = null!;
    }

    public record CreateOrderDelieryDetailsDTO
    {
        //public Ulid OrderDeliveryDetailId { get; set; }
        //public Ulid OrderDeliveryId { get; set; }
        public Ulid ProductId { get; set; }
        public DateTime DeliveredAt { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductUnit { get; set; } = null!;
    }
}
