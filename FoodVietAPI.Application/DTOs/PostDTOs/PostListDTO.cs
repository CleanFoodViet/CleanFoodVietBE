namespace CleanFoodVietAPI.Application.DTOs.PostDTOs
{
    public record PostListDTO
    (
        Ulid PostId,
        string Title,
        decimal Price,
        string Currency,
        string ThumbNail,
        string GardenerName,
        string GardenerAvatar
    );
}
