namespace CleanFoodVietAPI.Application.DTOs.OrderDTOs
{
    public class CheckOrderDetailDeliveryDTO
    {
        public Ulid OrderDetailId { get; set; }
        public int RemainDeliveryQuantity { get; set; }
    }
}
