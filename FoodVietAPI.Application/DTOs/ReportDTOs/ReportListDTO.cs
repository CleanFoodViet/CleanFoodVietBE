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
