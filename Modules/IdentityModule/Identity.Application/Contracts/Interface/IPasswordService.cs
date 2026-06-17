using Identity.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Contracts.Interface
{
    public interface IPasswordService
    {

        Task<string> ForgotPasswordAsync(ForgetPasswordDTO forgetPasswordDTO);
        Task<string> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
        Task<string> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO);

    }
}
