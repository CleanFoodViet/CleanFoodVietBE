using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Data.Entities
{
    public class Report
    {
        [Key]
        public Ulid ReportId { get; set; }
        public string ReportType { get; set; } = null!;
        public Ulid TargetId { get; set; }
        public string TargetType { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string? Description { get; set; }
        public string Severity { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Ulid AccountId { get; set; }

        public virtual Account Account { get; set; } = null!;
    }
}
