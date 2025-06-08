using CleanFoodVietAPI.Application.DTOs.AuthDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Login([FromBody] LoginDTO data)
        {
            var res = await _accountService.Login(data);

            return Ok(res);
        }

        [HttpPost(ApiEndpointConstant.Authentication.RegisterEndpoint)]
        [ProducesResponseType(typeof(AuthDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO data)
        {
            var res = await _accountService.Register(data);

            return Ok(res);
        }

        //Gardener Registration?
    }
}
