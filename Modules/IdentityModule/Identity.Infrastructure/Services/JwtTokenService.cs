using Identity.Domain.Interface.Service;
using Identity.Infrastructure.Configurations;
using Identity.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Services
{
    internal class JwtTokenService : IJwtTokenService
    {
        public readonly UserManager<User> _userManager;

        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _secretKey;
        private readonly string? _validIssuer;
        private readonly string? _validAudience;
        private readonly double _expires;

        public JwtTokenService(IConfiguration configuration, UserManager<User> userManager)
        {
            _userManager = userManager;

            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>(); // mapping

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

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user?.Id ?? string.Empty),
                new Claim(ClaimTypes.Name, user?.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user?.Email.Value ?? string.Empty)
            };

           
            var roles = await _userManager.GetRolesAsync(user);
           
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

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




        public string GenerateRefreshToken()
        {
            throw new NotImplementedException();
        }
    }
}
