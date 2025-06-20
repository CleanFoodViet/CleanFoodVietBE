using CleanFoodVietAPI.Application.DTOs.PostMediaDTOs;
using CleanFoodVietAPI.Application.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.PostDTOs
{
    public record PostDTO
    {
        // Post Data Field
        public Ulid PostId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime HarvestDate { get; set; }
        public string PostStatus { get; set; } = null!;
        public decimal Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime PostEndDate { get; set; }
        public int Priority { get; set; }

        // Post Media Data Field
        public List<PostMediaDTO>? PostMedias { get; set; }

        // Gardener Data Field
        public Ulid GardenerId { get; set; }
        public string GardenderName { get; set; } = null!;
        public string GardenerAvatar { get; set; } = null!;

        // Product Data Field
        public Ulid ProductId { get; set; }
        public PostProductDTO? ProductData { get; set; }
    }   
}
