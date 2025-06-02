using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs.AccountDTO
{
    public record AccountDTO
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
};

