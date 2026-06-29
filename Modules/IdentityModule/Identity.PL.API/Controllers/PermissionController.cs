using Identity.Application.Contracts.Interface;
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
        private readonly IPermissionService _permissionService;
        public PermissionController(IPermissionService service)
        {
            _permissionService = service;
        }
        // [Authorize]
        [HttpPost("assign/permission")]
        public async Task<IActionResult> AssignPermission([FromBody] AssignPermissionDTO dto)
        {
            await _permissionService.AssignPermissionAsync(dto);
            return Ok("Permission assigned to Role successfully");
        }

       // [Authorize]
        [HttpDelete("remove/permission")]
        public async Task<IActionResult> RemovePermission([FromBody] AssignPermissionDTO request) 
        {
            await _permissionService.RemovePermissionAsync(request);
            return Ok("Permission Removed from Role");
        }

       // [Authorize]
        [HttpGet("get/permissions")]
        public async Task<IActionResult> GetPermission([FromQuery] string rolename) 
        {
            var result = await _permissionService.GetPermissionsAsync(rolename);
            return Ok(result); 
        }
    }
}
