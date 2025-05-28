using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.AuthDTO
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
