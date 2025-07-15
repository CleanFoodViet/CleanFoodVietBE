using CleanFoodVietAPI.Application.DTOs.NotificationDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        [HttpGet(ApiEndpointConstant.Account.AccountNotificationEndpoint)]
        [ProducesResponseType(typeof(List<NotificationDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Get account's notification List")]
        public async Task<IActionResult> GetAccountNotification([FromRoute] string id)
        {
            var res = await _notificationService.GetNotificationList(id);
            return Ok(res);
        }

        [HttpPost(ApiEndpointConstant.Notification.NotificationsEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Create a Notification")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDTO request)
        {
            await _notificationService.CreateNotification(request);
            return Ok("Create Notification Successfully");
        }

        [HttpPatch(ApiEndpointConstant.Notification.NotificationEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Mark a Notification to read")]
        public async Task<IActionResult> UpdateNotificationToRead([FromRoute] string id)
        {
            await _notificationService.ReadNotification(id);
            return Ok("Create Notification Successfully");
        }
    }
}
