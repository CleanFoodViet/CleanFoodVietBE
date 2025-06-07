using Microsoft.AspNetCore.Mvc;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class ReportController : BaseController<ReportController>
    {
        public ReportController(ILogger<ReportController> logger) : base(logger)
        {
        }
    }
}
