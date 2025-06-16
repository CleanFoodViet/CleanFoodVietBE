using Microsoft.AspNetCore.Mvc;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class ChatMessageController : BaseController<ChatMessageController>
    {
        public ChatMessageController(ILogger<ChatMessageController> logger) : base(logger)
        {
        }
    }
}
