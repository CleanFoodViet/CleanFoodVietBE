namespace CleanFoodVietAPI.Application.DTOs.PostMediaDTOs
{
    public record PostMediaDTO
    (
        Ulid PostMediaId,
        string MediumUrl,
        string MediumType,
        DateTime UploadedAt
    );
}
