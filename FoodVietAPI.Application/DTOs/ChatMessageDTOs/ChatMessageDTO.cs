namespace CleanFoodVietAPI.Application.DTOs.ChatMessageDTOs
{
    public record ChatMessageDTO
    {
        public Ulid SenderId { get; set; }
        public Ulid ReceiverId { get; set; }
        public string? MessageText { get; set; }
        public DateTime SentAt { get; set; }
        public string MessageStatus { get; set; } = null!;
    }
}