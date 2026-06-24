using Identity.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.PL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        [Authorize]
        [HttpPost("assign/permission")]
        public async Task<IActionResult> AssignPermission([FromBody] AssignPermissionDTO dto)
        {
            return Ok();
        }

        [Authorize]
        [HttpDelete("remove/permission")]
        public async Task<IActionResult> RemovePermission([FromBody] AssignPermissionDTO request) 
        {
            return Ok();
        }

        [Authorize]
        [HttpGet("get/permissions")]
        public async Task<IActionResult> GetPermission([FromQuery] string rolename) 
        { 
            return Ok(); 
        }
    }
}
