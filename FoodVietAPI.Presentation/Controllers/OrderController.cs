using Microsoft.AspNetCore.Mvc;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class OrderController : BaseController<OrderController>
    {
        public OrderController(ILogger<OrderController> logger) : base(logger)
        {
        }
    }
}
