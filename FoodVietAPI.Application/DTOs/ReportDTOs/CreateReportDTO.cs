using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Application.DTOs.ReportDTOs
{
    public record CreateReportDTO
    {
        public string ReportType { get; set; } = null!;
        public Ulid TargetId { get; set; }
        public string TargetType { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string? Description { get; set; }
        public string Severity { get; set; } = null!;
        [Required]
        public Ulid AccountId { get; set; }
    }

    public record CreateUserReportDTO
    {
        public string ReportType { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string TargetType { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string? Description { get; set; }
        public string Severity { get; set; } = null!;
        [Required]
        public Ulid AccountId { get; set; }
    }
}
