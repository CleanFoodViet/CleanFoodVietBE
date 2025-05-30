using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.AccountDTO
{
    public record GetAccountDTO
    {
        public string AccountId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string Avatar { get; set; } = null!;
        public string Status { get; set; } = null!;
        public bool IsVerified { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string RoleName { get; set; } = null!;
    }
}
