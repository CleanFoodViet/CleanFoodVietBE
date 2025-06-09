using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.AccountDTOs
{
    public record AccountDetailDTO
    (
        Ulid AccountId,
         string Email,
         string PhoneNumber,
         string Gender,
         string Avatar,
         string Status,
         bool IsVerified,
         DateTime UpdatedAt,
         string RoleName
    );
}
