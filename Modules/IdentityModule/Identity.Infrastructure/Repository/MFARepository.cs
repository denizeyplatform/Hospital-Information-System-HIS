using Identity.Application.Contracts.Interface.Repository;
using Identity.Application.DTOs;
using Identity.Domain.Interface.Service;
using Identity.Infrastructure.Models;
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
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        public MFARepository(UserManager<User> userManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }
        public async Task<bool> EnableMFA(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");
            bool isEnabled = await _userManager.GetTwoFactorEnabledAsync(user);

            var enable = false;
            if(user.TwoFactorEnabled == false)
            {
                enable = true;
            }
            user.TwoFactorEnabled = enable;

            await _userManager.UpdateAsync(user);
            return user.TwoFactorEnabled;
        }

        public async Task<UserResponseDTO?> VerifyMfaAsync(VerifyMfaRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                 throw new Exception("User not found");

                var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, request.Code);

            if (!isValid)
                throw new Exception("Invalid verification code");

            var token = await _jwtTokenService.GenerateToken(user.Id);

            return new UserResponseDTO
            {
                token = token
            };
        }
    }

}
