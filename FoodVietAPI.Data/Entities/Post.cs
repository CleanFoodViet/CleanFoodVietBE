using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Data.Entities
{
    public class Post
    {
        [Key]
        public Ulid PostId { get; set; }
        public Ulid GardenerId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime HarvestDate { get; set; }
        public Ulid ProductId { get; set; }
        public string Status { get; set; } = null!;
        public decimal Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime PostEndDate { get; set; }
        public int Priority { get; set; }


        public virtual Account Account { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public virtual ICollection<Favorite> Favorites { get; set; } = new HashSet<Favorite>();
        public virtual ICollection<PostMedia> PostMedias { get; set; } = new HashSet<PostMedia>();
    }
}
