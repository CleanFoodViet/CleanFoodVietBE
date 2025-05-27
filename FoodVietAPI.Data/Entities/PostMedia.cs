using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Data.Entities
{
    public class PostMedia
    {
        [Key]
        public Ulid PostMediaId { get; set; }
        public string MediumUrl { get; set; } = null!;
        public string MediumType { get; set; } = null!;
        public DateTime UploadedAt { get; set; }

        public virtual Post Post { get; set; } = null!;
    }
}
