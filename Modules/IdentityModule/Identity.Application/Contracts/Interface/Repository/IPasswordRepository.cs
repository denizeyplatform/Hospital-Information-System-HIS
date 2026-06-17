using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Contracts.Interface.Repository
{
    public interface IPasswordRepository
    {
        Task<string> ForgotPasswordAsync(string email);
        Task<string> ResetPasswordAsync(string Email, string Token, string NewPassword, string ConfirmPassword);
        Task<string> ChangePasswordAsync(string CurrentPassword, string NewPassword, string ConfirmPassword);
    }
}
