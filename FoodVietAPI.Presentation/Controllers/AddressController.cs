using Microsoft.AspNetCore.Mvc;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class AddressController : BaseController<AddressController>
    {
        public AddressController(ILogger<AddressController> logger) : base(logger)
        {
        }
    }
}
