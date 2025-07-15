namespace CleanFoodVietAPI.Application.DTOs.NotificationDTOs
{
    public record NotificationDTO
    {
        public Ulid NotificationId { get; set; }
        public string Message { get; set; } = null!;
        public string? Link { get; set; }
        public string Sender { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
