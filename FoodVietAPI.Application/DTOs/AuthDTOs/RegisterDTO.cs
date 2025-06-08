using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Application.DTOs.AuthDTOs
{
    public record RegisterDTO
    {
        [Required]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required] 
        public string Email { get; set; } = null!;
        public string? Gender { get; set; }
        public string? Avatar { get; set; }
    };
}
