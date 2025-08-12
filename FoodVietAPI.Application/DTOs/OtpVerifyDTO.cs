using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.DTOs
{
    public class OtpVerifyDTO
    {
        public string PhoneNumber { get; set; } = null!;
        public string Otp { get; set; } = null!;
    }
}
