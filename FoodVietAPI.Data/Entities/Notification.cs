using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class Notification
    {
        [Key]
        public Ulid NotificationId { get; set; }
        public Ulid AccountId { get; set; }
        public string Message { get; set; } = null!;
        public string? Link { get; set; }
        public string Sender { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Account Account { get; set; } = null!;
    }
}
    