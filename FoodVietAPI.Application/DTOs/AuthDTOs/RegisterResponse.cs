namespace CleanFoodVietAPI.Application.DTOs.AuthDTOs
{
    public record RegisterResponse
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
