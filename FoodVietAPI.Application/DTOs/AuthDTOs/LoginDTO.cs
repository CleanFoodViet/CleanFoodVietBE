using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Application.DTOs.AuthDTOs
{
    public record LoginDTO(
        [Required] string PhoneNumber,
        [Required] string Password
        );
}
