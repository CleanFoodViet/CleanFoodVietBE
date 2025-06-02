using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.AccountDTO
{
    public record UpdateAccountDTO
    {
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? Avatar { get; set; }
    }
}
