using Microsoft.AspNetCore.Mvc;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class AppointmentController : BaseController<AppointmentController>
    {
        public AppointmentController(ILogger<AppointmentController> logger) : base(logger)
        {
        }
    }
}
