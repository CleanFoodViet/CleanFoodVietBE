using CleanFoodVietAPI.Application.DTOs.PostMediaDTOs;
using System.ComponentModel.DataAnnotations;

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

        [Range(0, double.MaxValue, ErrorMessage = "Deposit Amount must be 0 or greater.")]
        public decimal DepositAmount { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Deposit Percentage must be 0 or greater.")]
        public int DepositPercentage { get; set; }

        public List<CreatePostMediaDTO>? postMediaDTOs { get; set; }
    }
}
