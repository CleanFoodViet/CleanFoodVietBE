using Microsoft.AspNetCore.Mvc;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : BaseController<PostController>
    {
        public PostController(ILogger<PostController> logger) : base(logger)
        {
        }
    }
}
