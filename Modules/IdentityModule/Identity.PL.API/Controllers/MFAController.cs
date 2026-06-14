using Identity.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.PL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MFAController : ControllerBase
    {
        [HttpPost("verify-mfa")]
        public async Task<IActionResult> VerifyMfa(VerifyMfaRequest request)
        {
            //var result =
            //    await _mediator.Send(
            //        new VerifyMfaCommand(
            //            request.UserId,
            //            request.Code));

            return Ok();
        }

        [Authorize]
        [HttpPost("enable-mfa")]
        public async Task<IActionResult> EnableMfa()
        {
            //var user =
            //    await _userManager.GetUserAsync(User);

            //user.IsMfaEnabled = true;

            //await _userManager.UpdateAsync(user);

            return Ok();
        }
    }
}
