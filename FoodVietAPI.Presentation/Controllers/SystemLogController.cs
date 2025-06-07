using Microsoft.AspNetCore.Mvc;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class SystemLogController : BaseController<SystemLogController>
    {
        public SystemLogController(ILogger<SystemLogController> logger) : base(logger)
        {
        }
    }
}
