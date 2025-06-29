using CleanFoodVietAPI.Application.DTOs.NotificationDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class NotificationController : BaseController<NotificationController>
    {
        private readonly INotificationService _notificationService;
        public NotificationController(ILogger<NotificationController> logger, INotificationService notificationService) : base(logger)
        {
            _notificationService = notificationService;
        }

        [HttpPost(ApiEndpointConstant.Notification.NotificationEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDTO request)
        {
            await _notificationService.CreateNotification(request);
            return Ok("Create Notification Successfully");
        }
    }
}
