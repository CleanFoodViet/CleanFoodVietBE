namespace CleanFoodVietAPI.Application.DTOs.Payment
{
    public class CreateCheckoutRequest
    {
        public Ulid GardenerId { get; set; }
        public Ulid ServicePackageId { get; set; }
        public int Quantity { get; set; } = 1;
        public string? Location { get; set; } = "VN";
    }
}
