using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.AuthDTO
{
    public record RegisterDTO(
        [Required] string PhoneNumber,
        [Required] string Password,
        [Required] string Email,
        string Gender,
        string Avatar
        );
}
