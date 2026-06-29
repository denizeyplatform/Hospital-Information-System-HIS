using Identity.Application.Contracts.Interface;
using Identity.Application.DTOs;
using Identity.Domain.Constants;
using Identity.Domain.Entities;
using Identity.Infrastructure.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Identity.PL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
 
        //[Authorize(Roles = "Admin")]
        //[Authorize(Policy = "User.Create")]
        [HasPermission(Permissions.Roles.Create)]
     
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO request)
        {
            var result = await _userService.CreateUser(request);

            return Ok(result);
        }

      //  [Authorize(Roles = "Admin")]
        [HttpPut("{userId}")]
        public async Task<IActionResult> Update(string userId, [FromBody] UpdateUserDTO updateUserDTO)
        {
             await _userService.UpdateUser(userId, updateUserDTO);

            return Ok("User Updated");
        }

      //  [Authorize(Roles = "Admin")]
        [HttpDelete("/deactivate/{id}")]
        public async Task<IActionResult> Deactivate(string id)
        {
            await _userService.DeactivateUser(id);
            return Ok("User Deactivated");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("activate/{id}")]
        public async Task<IActionResult> Activate(string id)
        {
            await _userService.ActivateUser(id);
            return Ok("User Activate");
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {

            return Ok("Email confirmed");
        }
    }
}
