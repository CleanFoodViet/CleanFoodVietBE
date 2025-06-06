using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class SystemLog
    {
        [Key]
        public Ulid LogId { get; set; }
        public Ulid AdminId { get; set; }
        public string Action { get; set; } = null!;
        public Ulid TargetId { get; set; }
        public string TargetType { get; set; } = null!;
        public string? Desctiption { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Account Admin { get; set; } = null!;
    }
}
