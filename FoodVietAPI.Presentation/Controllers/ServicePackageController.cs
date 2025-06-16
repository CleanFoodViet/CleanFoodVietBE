using Microsoft.AspNetCore.Mvc;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class ServicePackageController : BaseController<ServicePackageController>
    {
        public ServicePackageController(ILogger<ServicePackageController> logger) : base(logger)
        {
        }
    }
}
