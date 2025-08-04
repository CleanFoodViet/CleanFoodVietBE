using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.ReviewDTOs
{
    public class CreateReviewRequest
    {
        [Required] public string RetailerId { get; set; } = null!;
        [Required] public string OrderDetailId { get; set; } = null!;
        [Range(1, 5)] public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
