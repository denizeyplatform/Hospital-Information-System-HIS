using Identity.Application.Contracts.Interface.Repository;
using Identity.Application.DTOs;
using Identity.Domain.Interface.Service;
using Identity.Infrastructure.Configurations;
using Identity.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Identity.Infrastructure.Services
{
    internal class JwtTokenService : IJwtTokenService
    {
        public readonly UserManager<User> _userManager;
        public readonly IPermissionRepository _permissionRepository;
        private readonly IConfiguration? _configuration;
        private readonly SymmetricSecurityKey _secretKey;
        private readonly string? _validIssuer;
        private readonly string? _validAudience;
        private readonly double _expires;

        public JwtTokenService(IConfiguration configuration, UserManager<User> userManager,IPermissionRepository permissionRepository)
        {
            _configuration = configuration;
            _userManager = userManager;
            _permissionRepository = permissionRepository;

            var jwtSettings = _configuration.GetSection("JwtSettings").Get<JwtSettings>(); // mapping

            if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.SecretKey))
            {
                throw new InvalidOperationException("JWT secret key is not configured.");
            }

            _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            _validIssuer = jwtSettings.Issuer;
            _validAudience = jwtSettings.Audience;
            _expires = jwtSettings.Expires;
        }


    
        public async Task<string> GenerateToken(string userId)
        {
            //Header 
            var signingCredentials = Header();
            //Payload
            var payload = await GetClaimsPayloadAsync(userId);
            //Signature
            var token = await JWTSignature(signingCredentials, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private SigningCredentials Header()
        {
            return new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256); // header
        }

        private async Task<List<Claim>> GetClaimsPayloadAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException($"User with ID '{userId}' not found.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user?.Id ?? string.Empty),
                new Claim(ClaimTypes.Name, user?.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user?.Email ?? string.Empty),
                new Claim("security_stamp", user.SecurityStamp)
                
            };

            var roles = await _userManager.GetRolesAsync(user);
           
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

           
            var permissions = await _permissionRepository.GetPermissionsByUserAsync(user.Id);

            claims.AddRange(permissions.Select(permission => new Claim("Permission", permission)));

            //Use short-lived access tokens(for example, 10–15 minutes).
            //Use refresh tokens to obtain new access tokens.
            //When roles or permissions change:
            //            Revoke the user's refresh tokens.
            //            Update the user's security stamp (if you're using ASP.NET Identity).
            //            Force re-authentication if required.
            return claims;
        }

        private async Task<JwtSecurityToken> JWTSignature(SigningCredentials signingCredentials, List<Claim> claims)
        {
            return new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                issuer: _validIssuer,
                audience: _validAudience,
                expires: DateTime.Now.AddMinutes(_expires)
            );
        }


        public async Task<string> GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            var refreshToken = Convert.ToBase64String(randomNumber);


            // save refresh token to database with user association and expiration time
            // get current user from context
        
            var httpContext = new HttpContextAccessor().HttpContext;
            var userId = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userManager.FindByIdAsync(userId);


            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // example expiration time
           

            var result = await _userManager.UpdateAsync(user) ;
            if (result.Succeeded)
            {
                return refreshToken;

            }
            return "Invalid Refresh Token";
            
        }

       
    }
}
