using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.ReportDTOs
{
    public record ReportListDTO
    (
        Ulid ReportId,
        string ReportType,
        string TargetType,
        string Subject,
        string? Description,
        string Severity,
        string Status,
        DateTime CreatedAt
    );
}
