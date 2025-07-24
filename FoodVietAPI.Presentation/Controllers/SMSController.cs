using CleanFoodVietAPI.Application.DTOs;
using CleanFoodVietAPI.Application.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSController : ControllerBase
    {
        [HttpPost("SendText")]
        public async Task<IActionResult> SendText([FromBody]string phoneNumber)
        {
            // Format to +84 if needed
            if (phoneNumber.StartsWith("0"))
                phoneNumber = "+84" + phoneNumber.Substring(1);
            else if (!phoneNumber.StartsWith("+"))
                phoneNumber = "+84" + phoneNumber;

            var otp = new Random().Next(100000, 999999).ToString();

            using var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("number", phoneNumber),
                new KeyValuePair<string, string>("message", $"Mã OTP của bạn là: {otp}"),
                new KeyValuePair<string, string>("key", "t  extbelt")
            });

            var response = await client.PostAsync("http://localhost:9090/intl", content);
            var resultJson = await response.Content.ReadAsStringAsync();

            // Parse the JSON result and check success
            bool success = JsonDocument.Parse(resultJson)
                .RootElement
                .GetProperty("success")
                .GetBoolean();

            if (success)
            {
                OtpStore.Set(phoneNumber, otp);
                return Ok(new { success = true, message = "OTP sent successfully", otp }); // ⚠️ Return `otp` for dev only
            }

            return BadRequest(new { success = false, message = "Failed to send OTP", response = resultJson });
        }

        [HttpPost("VerifyOtp")]
        public IActionResult VerifyOtp([FromBody]OtpVerifyDTO request)
        {
            bool isValid = OtpStore.Validate(request.PhoneNumber, request.Otp);

            if (isValid)
                return Ok(new { success = true, message = "OTP verified" });

            return BadRequest(new { success = false, message = "Invalid or expired OTP" });
        }
    }
}
