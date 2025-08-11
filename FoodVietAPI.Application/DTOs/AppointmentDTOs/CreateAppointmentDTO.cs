using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Application.DTOs.AppointmentDTOs
{
    public record CreateAppointmentDTO
    {
        public Ulid GardenerId { get; set; }
        public Ulid RetailerId { get; set; }
        public DateTime AppointmentDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Duration must be at least 1 or greater.")]
        public int Duration { get; set; }
        public string Subject { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string AppointmentType { get; set; } = null!;
        public string? Description { get; set; }
    }
}
