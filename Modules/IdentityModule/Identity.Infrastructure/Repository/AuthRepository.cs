using Identity.Application.Contracts.Interface.Repository;
using Identity.Application.DTOs;
using Identity.Domain.Enums;
using Identity.Domain.Interface.Service;
using Identity.Domain.ValueObjects;
using Identity.Infrastructure.Models;
using Identity.Infrastructure.Presistance.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Infrastructure.Repository
{
    public class AuthRepository : IAuthRepository
    {
        public readonly AppIdentityDBContext _context;
        public readonly UserManager<User> userManager;
        public readonly IJwtTokenService jwtTokenService;
        public AuthRepository(AppIdentityDBContext context, UserManager<User> _userManager, IJwtTokenService _jwtTokenService)
        {
            _context = context;
            userManager = _userManager;
            jwtTokenService = _jwtTokenService;
        }

        public async Task<UserResponseDTO?> Login(string email, string password)
        {
           
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
               await userManager.CheckPasswordAsync(user, password);
              
               var token = await jwtTokenService.GenerateToken(user.Id);
              

                //if (user.TwoFactorEnabled)
                //{
                //    return new UserResponseDTO
                //    {
                //        RequiresMfa = true,
                //        UserId = user.Id
                //    };
                //}
                return new UserResponseDTO
               {
                   FirstName = user.FirstName,
                   Email = user.Email,
                 //  Gender = Enum.Parse<Gender>(user.Gender.ToString()),
                   token = token
                };
            }
            return null;
        }

        public async Task<UserResponseDTO> Register(string name, string email, string password,int gender ,string role)
        {
            
            var newUser = new User
            { 
                UserName = name,
                Email = email,
                PasswordHash = password,
                Gender = gender
            };

           
           var result =  await userManager.CreateAsync(newUser, newUser.PasswordHash);
            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, role);
                
                var newUser2 = await userManager.FindByEmailAsync(email);
                
                var token = await jwtTokenService.GenerateToken(newUser2.Id);

                return new UserResponseDTO
                {
                    FirstName = name,
                    Email = email,
                    Gender = Enum.Parse<Gender>(gender.ToString()),
                    token = token
                };
            }
            else
            {
                 throw new Exception("Registration failed.");

            }
        }

        public async Task<RevokeRefreshTokenResponse> RevokeRefreshToken(RefreshTokenRequest request)
        {
            // Hash the refresh token
            //using var sha256 = SHA256.Create();
            //var refreshTokenHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(request.RefreshToken));
            //var hashedRefreshToken = Convert.ToBase64String(refreshTokenHash);

            // Find the user based on the refresh token
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
            if (user == null)
            {
                throw new Exception("Invalid refresh token");
            }

            // Validate the refresh token expiry time
            if (user.RefreshTokenExpiryTime < DateTime.Now)
            {
                throw new Exception("Refresh token expired");
            }

            // Remove the refresh token
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            // Update user information in database
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {

                return new RevokeRefreshTokenResponse
                {
                    Message = "Failed to revoke refresh token"
                };
            }

            return new RevokeRefreshTokenResponse
            {
                Message = "Refresh token revoked successfully"
            };
        }
    }
}
