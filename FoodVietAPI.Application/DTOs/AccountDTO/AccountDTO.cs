namespace CleanFoodVietAPI.Application.DTOs.AccountDTO
{
    public record AccountDTO
    (
         Ulid AccountId,
         string Email,
         string PhoneNumber,
         string Gender,
         string Avatar,
         string Status,
         bool IsVerified,
         DateTime UpdatedAt,
         string RoleName
    );
};

