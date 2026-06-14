using Identity.Application.DTOs;
using Identity.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Contracts.Interface.Repository
{
    public interface IAuthRepository
    {
        Task<UserResponseDTO> Register(string name , string email , string password,int gender ,string role);

        Task<UserResponseDTO> Login(string email , string password);
        // logout
        Task<RevokeRefreshTokenResponse> RevokeRefreshToken(RefreshTokenRequest refreshTokenRemoveRequest);

    }
}
