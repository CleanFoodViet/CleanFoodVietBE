using CleanFoodVietAPI.Application.DTOs.AuthDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class AuthenticationController : BaseController<AuthenticationController>
    {
        private readonly IAccountService _accountService;

        public AuthenticationController(ILogger<AuthenticationController> logger, IAccountService accountService) : base(logger)
        {
            _accountService = accountService;
        }

        [HttpPost(ApiEndpointConstant.Authentication.LoginEndpoint)]
        [ProducesResponseType(typeof(AuthDTO), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Login function for all Role.")]
        public async Task<IActionResult> Login([FromBody] LoginDTO data)
        {
            var res = await _accountService.Login(data);

            return Ok(res);
        }

        [HttpPost(ApiEndpointConstant.Authentication.RegisterEndpoint)]
        [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Regsiter Function for Retailer")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO data)
        {
            var res = await _accountService.Register(data);

            return StatusCode(StatusCodes.Status201Created, res);
        }

        //Gardener Registration?
        [HttpPost(ApiEndpointConstant.Authentication.GardenerRegisterEndpoint)]
        [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Regsiter Function for Gardener")]
        public async Task<IActionResult> GardenerRegister([FromBody] GardenerRegisterDTO data)
        {
            var res = await _accountService.GardenerRegister(data);

            return StatusCode(StatusCodes.Status201Created, res);
        }

        //Admin Registration/createion
        [HttpPost(ApiEndpointConstant.Authentication.AdminRegisterEndpoint)]
        [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
        [SwaggerOperation(Summary = "Create Admin Accoun function.")]
        public async Task<IActionResult> AdminRegister([FromBody] RegisterDTO data)
        {
            var res = await _accountService.CreateAdmin(data);

            return StatusCode(StatusCodes.Status201Created, res);
        }
    }
}
