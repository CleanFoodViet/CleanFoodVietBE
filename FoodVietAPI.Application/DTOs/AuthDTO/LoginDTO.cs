using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Application.DTOs.AuthDTO
{
    public record LoginDTO(
        [Required] string PhoneNumber,
        [Required] string Password
        );
}
