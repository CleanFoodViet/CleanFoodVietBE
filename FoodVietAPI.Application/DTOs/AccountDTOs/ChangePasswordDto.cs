using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.AccountDTOs
{
    public record ChangePasswordDto
    {
        public string Action { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? OldPassword { get; set; }
        public string NewPassword { get; set; } = null!;
        public string ConfirmNewPassword { get; set; } = null!;
    }
}
