using CleanFoodVietAPI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class AddressController : BaseController<AddressController>
    {
        private readonly IAddressService _addressService;

        public AddressController(ILogger<AddressController> logger, IAddressService addressService) : base(logger)
        {
            _addressService = addressService;
        }
    }
}
