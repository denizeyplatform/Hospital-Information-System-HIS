using Identity.Application.Contracts.Interface;
using Identity.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Identity.PL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MFAController : ControllerBase
    {
        public readonly IMFAService _mfaService;
        public MFAController(IMFAService mfaService)
        {
            _mfaService = mfaService;
        }

        [Authorize]
        [HttpPost("enable-mfa")]
        public async Task<IActionResult> EnableMfa(string userId)
        {
            var result = await _mfaService.EnableMFAAsync(userId);

            return Ok(result);
        }

        [HttpPost("verify-mfa")]
        public async Task<IActionResult> VerifyMfa(VerifyMfaRequest request)
        {
            var result = await _mfaService.VerifyMfaAsync(request);

            return Ok(result);
        }

       
    }
}
