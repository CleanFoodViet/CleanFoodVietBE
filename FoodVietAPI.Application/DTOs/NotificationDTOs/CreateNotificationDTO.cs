namespace CleanFoodVietAPI.Application.DTOs.NotificationDTOs
{
    public record CreateNotificationDTO
    {
        public Ulid AccountId { get; set; }
        public string Message { get; set; } = null!;
        public string? Link { get; set; }
        public bool IsRead { get; set; }
        public string Sender { get; set; } = null!;
    }
}
