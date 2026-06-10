using Identity.Domain.Interface.Repository;
using Identity.Domain.ValueObjects;
using Identity.Infrastructure.Models;
using Identity.Infrastructure.Presistance.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Repository
{
    public class AuthRepository : IAuthRepository
    {
        public readonly AppIdentityDBContext _context;
        public readonly UserManager<User> userManager;
        public AuthRepository(AppIdentityDBContext context, UserManager<User> _userManager)
        {
            _context = context;
            userManager = _userManager;
        }

        public async  Task<object> Login(string email, string password)
        {
            User.login();
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
               await userManager.CheckPasswordAsync(user, password);
              //  user.token = generateTokent();
                return user;
            }
            return null;
        }

        public async Task<object> Register(string name, string email, string password)
        {
            User.register();
            var newUser = new User
            {
                UserName = name,
                Email = email,
                hashedPassword = new HashedPassword(password) { Value = password },
            };

           
           var result =  await userManager.CreateAsync(newUser, newUser.hashedPassword.Value);
            if(result.Succeeded)
            {
                var newUser2 = userManager.FindByEmailAsync(email);
                return newUser2;
            }
            else
            {
                return result.Errors;

            }
        }

        
    }
}
