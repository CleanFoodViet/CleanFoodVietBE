namespace CleanFoodVietAPI.Application.DTOs.AppointmentDTOs
{
    public record GetRequestAppointmentDTO
    {
        public Ulid AppointmentId { get; set; }
        public string RetailerName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Avatar { get; set; } = null!;

        public string Subject { get; set; } = null!;
        public string? Description { get; set; }
        public string AppointmentType { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime AppointmentDate { get; set; }
        public int Duration { get; set; }
        public string Location { get; set; } = null!;
    }
}
