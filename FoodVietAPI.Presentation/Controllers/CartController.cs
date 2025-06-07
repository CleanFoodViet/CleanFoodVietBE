using Microsoft.AspNetCore.Mvc;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class CartController : BaseController<CartController>
    {
        public CartController(ILogger<CartController> logger) : base(logger)
        {
        }
    }
}
