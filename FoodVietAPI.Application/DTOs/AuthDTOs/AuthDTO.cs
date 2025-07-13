namespace CleanFoodVietAPI.Application.DTOs.AuthDTOs
{
    public record AuthDTO(
        string Token,
        string AccountId,
        string Name,
        string Avatar
        );
}
