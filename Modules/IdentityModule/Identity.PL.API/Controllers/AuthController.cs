using Identity.Application.Contracts.Interface;
using Identity.Application.DTOs;
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

        public async Task<IActionResult> Register(RegisterRequestDTO userDto)
        {
            var user = await _authService.RegisterAsync(userDto);
            if(user == null) return BadRequest("Invalid Registration Data");

            return Ok(user);
        }

        [HttpPost("login")]

        public async Task<IActionResult> login(LoginRequestDTO userDto)
        {
            var user = await _authService.LoginAsync(userDto);
            if(user == null) return BadRequest("Invalid Login Data");

            return Ok(user);
        }
    }

}
