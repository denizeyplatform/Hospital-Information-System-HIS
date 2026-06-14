using Identity.Application.Contracts.Interface.Repository;
using Identity.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Repository
{
    public class MFARepository : IMFARepository
    {
        public Task<bool> EnableMFA(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> VerifyMfaAsync(VerifyMfaRequest request)
        {
            //var user =
            //        await _userManager.FindByIdAsync(request.UserId);

            //var isValid =
            //    await _userManager.VerifyTwoFactorTokenAsync(user,TokenOptions.DefaultEmailProvider,request.Code);

            //if (!isValid)
            //    throw new UnauthorizedAccessException();

            //var token =
            //    _jwtService.GenerateToken(user);

            //return new AuthResponse
            //{
            //    AccessToken = token
            //};

            throw new NotImplementedException();
        }
    }
}
