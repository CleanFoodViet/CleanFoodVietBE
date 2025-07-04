namespace CleanFoodVietAPI.Application.DTOs.PostDTOs
{
    public record UpdatePostDTO
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
    }
}
