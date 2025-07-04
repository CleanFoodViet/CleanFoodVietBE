namespace CleanFoodVietAPI.Application.DTOs.AppointmentDTOs
{
    public record UpdateAppointmentDTO
    {
        public DateTime AppointmentDate { get; set; }
        public string Subject { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string AppointmentType { get; set; } = null!;
        public string? CancelledBy { get; set; }
        public string? CancellationReason { get; set; }
    }
}
