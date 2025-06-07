using Microsoft.AspNetCore.Mvc;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class ProductController : BaseController<ProductController>
    {
        public ProductController(ILogger<ProductController> logger) : base(logger)
        {
        }
    }
}
