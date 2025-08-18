using CleanFoodVietAPI.Application.DTOs.PostMediaDTOs;

namespace CleanFoodVietAPI.Application.DTOs.PostDTOs
{
    public record CreatePostDTO
    {
        //public Ulid PostId { get; set; }
        public Ulid GardenerId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime HarvestDate { get; set; }
        public Ulid ProductId { get; set; }

        //public decimal Rating { get; set; }
        //public DateTime CreatedAt { get; set; }
        public DateTime PostEndDate { get; set; }
        //public int Priority { get; set; }
        public string HarvestStatus { get; set; } = null!;
        public decimal DepositAmount { get; set; }
        public int DepositPercentage { get; set; }

        public List<CreatePostMediaDTO>? postMediaDTOs { get; set; }
    }
}
