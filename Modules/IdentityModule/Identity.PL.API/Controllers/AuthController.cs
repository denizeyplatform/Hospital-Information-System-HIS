using Identity.Application.Contracts.Interface;
using Identity.Application.DTOs;
using Identity.PL.API.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.PL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]

        public async Task<IActionResult> Register(RegisterRequestDTO userDto)
        {
            var user = await _authService.RegisterAsync(userDto);
            if(user == null) return BadRequest("Invalid Registration Data");

            return Ok(user);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ApiKey]

        public async Task<IActionResult> login(LoginRequestDTO userDto)
        {
            var user = await _authService.LoginAsync(userDto);
            if(user == null) return BadRequest("Invalid Login Data");

            return Ok(user);
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            var token = await _authService.RefreshToken();
            if (token == null) return BadRequest("Invalid Token");
            return Ok(token);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
        {
            var response = await _authService.LogoutAsync(request);
            if (response != null && response.Message == "You have logout successfully")
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

    }
}
