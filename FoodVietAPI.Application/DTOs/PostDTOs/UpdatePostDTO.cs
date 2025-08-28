namespace CleanFoodVietAPI.Application.DTOs.PostDTOs
{
    public record UpdatePostDTO
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string HarvestStatus { get; set; } = null!;
        public DateTime? HarvestDate { get; set; }
        public DateTime? PostEndDate { get; set; }
    }
}
