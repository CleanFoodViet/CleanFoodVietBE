namespace CleanFoodVietAPI.Application.DTOs.AccountDTOs
{
    public record UpdateAccountDTO
    {
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? Avatar { get; set; }
        public string? Bio { get; set; }
    }
}
