using Microsoft.AspNetCore.Mvc;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class SubscriptionContractController : BaseController<SubscriptionContractController>
    {
        public SubscriptionContractController(ILogger<SubscriptionContractController> logger) : base(logger)
        {
        }
    }
}
