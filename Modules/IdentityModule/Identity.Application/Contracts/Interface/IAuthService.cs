using Identity.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Contracts.Interface
{
    public interface IAuthService
    {
        Task<UserResponseDTO> RegisterAsync(RegisterRequestDTO request);
        Task<UserResponseDTO> LoginAsync(LoginRequestDTO request);
        Task<string> RefreshToken();
        Task<RevokeRefreshTokenResponse> LogoutAsync(RefreshTokenRequest refreshTokenRemoveRequest);

    }
}
