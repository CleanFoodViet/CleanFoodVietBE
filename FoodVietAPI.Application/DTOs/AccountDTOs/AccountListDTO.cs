namespace CleanFoodVietAPI.Application.DTOs.AccountDTOs
{
    public record AccountListDTO
    (
        Ulid AccountId,
         string Email,
         string PhoneNumber,
         string Gender,
         string Avatar,
         string Status,
         bool IsVerified
    );
}
