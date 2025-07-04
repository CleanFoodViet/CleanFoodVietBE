namespace CleanFoodVietAPI.Application.DTOs.AppointmentDTOs
{
    public record AppointmentListDTO
    {
        public Ulid AppointmentId { get; set; }
        public Ulid GardenerId { get; set; }
        public Ulid RetailerId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Subject { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string AppointmentType { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? CancelledBy { get; set; }
        public string? CancellationReason { get; set; }
    }
}
