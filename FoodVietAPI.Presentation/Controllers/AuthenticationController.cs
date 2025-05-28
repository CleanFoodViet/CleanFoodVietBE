using CleanFoodVietAPI.Application.DTOs;
using CleanFoodVietAPI.Application.DTOs.AuthDTO;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [Route(ApiEndpointConstant.ApiEndpoint)]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AuthenticationController(IAccountService accountService)
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
