using Identity.Application.Contracts.Interface;
using Identity.Application.Contracts.Interface.Repository;
using Identity.Domain.Entities;
using Identity.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Repository
{
    public class PasswordRepository : IPasswordRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly ICurrentUserService _currentUser;
        public PasswordRepository(UserManager<User> userManager, IEmailService emailService, ICurrentUserService currentUser)
        {
            _userManager = userManager;
            _emailService = emailService;
            _currentUser = currentUser;
        }
        public async Task<string> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                throw new Exception("User not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken =WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var resetUrl =  $"https://localhost:7000/api/Password/reset-password?email={user.Email}&token={encodedToken}";

            await _emailService.SendAsync(user.Email!,"Reset Password",$"Reset your password: {resetUrl}");

            return "Reset link sent.";

        }

        public async Task<string> ResetPasswordAsync(string Email, string Token, string NewPassword, string ConfirmPassword)
        {
            if (NewPassword != ConfirmPassword)
                return "Passwords do not match";

            var user =
                await _userManager.FindByEmailAsync(Email);

            if (user == null)
                return "User not found";

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Token));

            var result = await _userManager.ResetPasswordAsync(user,decodedToken,NewPassword);

            if (!result.Succeeded)
                return 
                    string.Join(",", result.Errors.Select(x => x.Description));

            return "Password reset successfully";

        }
        public async Task<string> ChangePasswordAsync(string CurrentPassword, string NewPassword, string ConfirmPassword)
        {
            if (NewPassword != ConfirmPassword)
                return "Passwords do not match";

            var user = await _userManager.FindByIdAsync(_currentUser.UserId);

            if (user == null)
                return "User not found";

            var result = await _userManager.ChangePasswordAsync(user,CurrentPassword,NewPassword);

            if (!result.Succeeded)
                return string.Join(",",result.Errors.Select(x => x.Description));


            return "Password changed successfully";
        }
    }
}
