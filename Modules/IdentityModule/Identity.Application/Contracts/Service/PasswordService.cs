using Identity.Application.Contracts.Interface;
using Identity.Application.Contracts.Interface.Repository;
using Identity.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Contracts.Service
{
    public class PasswordService : IPasswordService
    {
        private readonly IPasswordRepository _passwordRepository;
        public PasswordService(IPasswordRepository passwordRepository)
        {
            _passwordRepository = passwordRepository;
        }

        public async Task<string> ForgotPasswordAsync(ForgetPasswordDTO forgetPasswordDTO)
        {
            return await _passwordRepository.ForgotPasswordAsync(forgetPasswordDTO.Email);
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            return await _passwordRepository.ResetPasswordAsync(
                resetPasswordDTO.Email,
                resetPasswordDTO.Token,
                resetPasswordDTO.NewPassword,
                resetPasswordDTO.ConfirmPassword
            );
        }
        public async Task<string> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO)
        {
            return await _passwordRepository.ChangePasswordAsync(
                changePasswordDTO.CurrentPassword,
                changePasswordDTO.NewPassword,
                changePasswordDTO.ConfirmPassword
            );
        }
     
    }
}
