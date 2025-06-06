using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class Favorite
    {
        [Key]
        public Ulid FavoriteId { get; set; }
        public Ulid RetailerId { get; set; }
        public Ulid PostId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual Account Account { get; set; } = null!;
    }
}
