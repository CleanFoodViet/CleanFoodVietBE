using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.PostMediaDTOs
{
    public record CreatePostMediaDTO
    {
        public string MediumUrl { get; set; } = null!;
        public string MediumType { get; set; } = null!;
        public DateTime UploadedAt { get; set; }
        public Ulid PostId { get; set; }
    }
}
