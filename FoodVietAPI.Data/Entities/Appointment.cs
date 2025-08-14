using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Data.Entities
{
    public class Appointment
    {
        [Key]
        public Ulid AppointmentId { get; set; }
        public Ulid GardenerId { get; set; }
        public Ulid RetailerId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int Duration { get; set; }
        public string Subject { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string AppointmentType { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? ActionedBy { get; set; }
        public string? ActionReason { get; set; }

        public virtual Account Gardener { get; set; } = null!;
        public virtual Account Retailer { get; set; } = null!;
    }
}
