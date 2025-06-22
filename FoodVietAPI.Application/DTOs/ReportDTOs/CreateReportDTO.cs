using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
