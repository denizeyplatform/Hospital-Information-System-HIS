using Identity.Application.Contracts.Interface;
using Identity.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.PL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        //[Authorize(Policy = "Role.Create")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            var roleId = await _roleService.CreateRoleAsync(request);

            return CreatedAtAction(
                nameof(CreateRole),
                new { id = roleId },
                new
                {
                    RoleId = roleId,
                    Message = "Role created successfully"
                });
        }

        [HttpPost("claims")]
       // [Authorize(Policy = "Role.ManageClaims")]
        public async Task<IActionResult> AddClaim([FromBody] RoleClaimRequest request)
        {
            await _roleService.AddClaimAsync(request.roleName, request);

            return Ok(new
            {
                Message = "Claim added successfully"
            });
        }

        [HttpDelete("/role/claims")]
       // [Authorize(Policy = "Role.ManageClaims")]
        public async Task<IActionResult> RemoveClaim([FromBody] RoleClaimRequest request)
        {
            await _roleService.RemoveClaimAsync(request.roleName, request);

            return Ok(new
            {
                Message = "Claim removed successfully"
            });
        }

        [HttpPost("{userId}/roles")]
      //  [Authorize(Policy = "Role.Assign")]
        public async Task<IActionResult> AssignRole(string userId,[FromBody] AssignRoleRequest request)
        {
            await _roleService.AssignRoleAsync(userId, request);

            return Ok(new
            {
                Message = "Role assigned successfully"
            });
        }

        [HttpDelete("{userId}/roles/{roleName}")]
      //  [Authorize(Policy = "Role.Remove")]
        public async Task<IActionResult> RemoveRole(string userId,string roleName)
        {
            await _roleService.RemoveRoleAsync(userId,roleName);

            return Ok(new
            {
                Message = "Role removed successfully"
            });
        }

        [HttpGet("{userId}/roles")]
       // [Authorize(Policy = "Role.View")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var roles =
                await _roleService.GetUserRolesAsync(userId);

            return Ok(roles);
        }
    }
}
