using Microsoft.AspNetCore.Mvc;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class FavoriteController : BaseController<FavoriteController>
    {
        public FavoriteController(ILogger<FavoriteController> logger) : base(logger)
        {
        }
    }
}
