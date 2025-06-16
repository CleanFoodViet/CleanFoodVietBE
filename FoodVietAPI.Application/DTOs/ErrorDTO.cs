namespace CleanFoodVietAPI.Application.DTOs
{
    public record ErrorDTO
    (
        int StatusCode,
        string Error,
        DateTime TimeStamp
    );
}
