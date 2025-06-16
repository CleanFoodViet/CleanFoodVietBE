using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.ReportDTOs
{
    public record ReportDTO
    (
        Ulid ReportId,
        string ReportType,
        Ulid TargetId,
        string TargetType,
        string Subject,
        string? Description,
        string Severity,
        string Status,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        string AccountPhoneNumber,
        string AccountAvatar,
        string Role
    );
}
