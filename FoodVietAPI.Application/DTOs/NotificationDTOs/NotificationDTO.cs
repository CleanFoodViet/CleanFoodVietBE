namespace CleanFoodVietAPI.Application.DTOs.NotificationDTOs
{
    public record NotificationDTO
    {
        public string Message { get; set; } = null!;
        public string? Link { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
