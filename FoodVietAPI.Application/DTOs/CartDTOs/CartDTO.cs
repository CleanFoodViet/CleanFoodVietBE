namespace CleanFoodVietAPI.Application.DTOs.CartDTOs
{
    public record CartDTO
    {
        //Cart Data Field
        public Ulid? CartId { get; set; }
        public Ulid RetailerId { get; set; }
        public Ulid GardenerId { get; set; }
        public string GardenerName { get; set; } = null!;
        //public DateTime UpdatedAt { get; set; }

        //Cart Itens
        public List<CartItemDTO> CartItems { get; set; } = null!;
    }
}
