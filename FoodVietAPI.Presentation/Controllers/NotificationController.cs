using Microsoft.AspNetCore.Mvc;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class NotificationController : BaseController<NotificationController>
    {
        public NotificationController(ILogger<NotificationController> logger) : base(logger)
        {
        }
    }
}
