using Identity.Domain.Interface.Service;
using Identity.Domain.ValueObjects;
using Identity.Infrastructure.Models;
using Identity.Infrastructure.Presistance.Data;
using Microsoft.AspNetCore.Identity;
using Identity.Application.Contracts.Interface;
using Identity.Application.DTOs;
using Identity.Domain.Enums;

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

        public async Task<UserResponseDTO> Login(string email, string password)
        {
            //User.login();
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
               await userManager.CheckPasswordAsync(user, password);
              
               var token = await jwtTokenService.GenerateToken(user.Id);
               user.token = token;

               return new UserResponseDTO
               {
                   FirstName = user.FirstName,
                   Email = user.Email.Value,
                   Gender = Enum.Parse<Gender>(user.Gender.ToString()),
                   token = user.token
               };
            }
            return null;
        }

        public async Task<UserResponseDTO> Register(string name, string email, string password)
        {
            User.register();
            var newUser = new User
            { 
                UserName = name,
                Email = new Email(email),
                HashedPassword = new HashedPassword(password) { Value = password },
            };

           
           var result =  await userManager.CreateAsync(newUser, newUser.HashedPassword.Value);
            if(result.Succeeded)
            {
                var newUser2 = await userManager.FindByEmailAsync(email);
                
                newUser2.token = await jwtTokenService.GenerateToken(newUser2.Id);

                return new UserResponseDTO
                {
                    FirstName = newUser2.FirstName,
                    Email = newUser2.Email.Value,
                    Gender = Enum.Parse<Gender>(newUser2.Gender.ToString()),
                    token = newUser2.token
                };
            }
            else
            {
                 throw new Exception("Registration failed.");

            }
        }

        
    }
}
