using Identity.Application.Contracts.Interface.Repository;
using Identity.Application.DTOs;
using Identity.Domain.Entities;
using Identity.Domain.ValueObjects;
using Identity.Infrastructure.Models;
using Identity.Infrastructure.Presistance.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Identity.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        public readonly UserManager<User> _userManager;
        public readonly AppIdentityDBContext _context;
        public UserRepository(UserManager<User> userManager, AppIdentityDBContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<Guid> CreateUser(CreateUserDTO createUserDTO)
        {
            // Assign roles during creation done
            // Send welcome email done
            // Require email confirmation done
            // Generate temporary password optional
            
            // Enrprise scenarios:
            //Create user without a password.
            //Generate a password setup token.
            //Send a "Set Your Password" link.
            //User chooses their own password.

            var existingUser = await _userManager.FindByEmailAsync(createUserDTO.Email);

            if (existingUser != null)
                throw new ApplicationException("Email already exists");

           // var password = $"Tmp@{Random.Shared.Next(100000, 999999)}";

            var user = new User
            {
                FirstName = createUserDTO.FirstName,
                LastName = createUserDTO.LastName,
                Email = createUserDTO.Email,
                UserName = createUserDTO.Email
               // PasswordHash = password
                
            };
           

            var result = await _userManager.CreateAsync(user, createUserDTO.HashedPassword);
        
            if(createUserDTO.Role != null)
            {
                await _userManager.AddToRoleAsync(user, createUserDTO.Role);
            }

            // done

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var confirmationLink =  $"https://localhost:7000/api/User/confirm-email" + $"?userId={user.Id}" + $"&token={encodedToken}";

            if (!result.Succeeded)
            {
                throw new ApplicationException(
                    string.Join(", ",
                    result.Errors.Select(x => x.Description)));
            }

            // raise domain event User.Create() in application layer to send email confirmation
            // step 1
            var userdomain = ApplicationUser.create(user.Id, user.Email, user.FirstName, user.LastName, createUserDTO.HashedPassword, confirmationLink);

            // step 2
            await _context.dispachEvents();

            // step 3 
            // handler send email or anthing else

            return Guid.Parse(userdomain.Id);
        }

        public async Task ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new ApplicationException("User not found");

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ConfirmEmailAsync(user,decodedToken);

            if (!result.Succeeded)
                throw new ApplicationException(
                    string.Join(", ",
                    result.Errors.Select(x => x.Description)));
        }

        public async Task UpdateUser(string userId, UpdateUserDTO updateUserDTO)
        {
            // Update roles
            // Update claims
            // Update permissions
            var user =
            await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new ApplicationException("User not found");

            user.FirstName = updateUserDTO.FirstName;
            user.LastName = updateUserDTO.LastName;
            user.PhoneNumber = updateUserDTO.PhoneNumber;
            user.UserName = updateUserDTO.UserName;
            user.NormalizedUserName = updateUserDTO.UserName;
            user.Email = updateUserDTO.Email?? user.Email;
            user.NormalizedUserName = updateUserDTO.NormalizedEmail;
            

            var result =
                await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new ApplicationException(
                    string.Join(", ",
                    result.Errors.Select(x => x.Description)));
            }
        }

        public async Task DeactivateUser(string UserId)
        {
            //Revoke refresh tokens done
            //Update security stamp done
            //Record audit log
            //Notify the user

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
                throw new ApplicationException("User not found");


            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
           
            await _userManager.UpdateSecurityStampAsync(user);

            await _userManager.UpdateAsync(user);

            // raise domain event User.Deactivate() in application layer to send account deactivation email
            //await _emailService.SendAsync(
            //        user.Email!,
            //        "Account Deactivated",
            //        "Your account has been deactivated.");
        }

        public async Task ActivateUser(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
                throw new ApplicationException("User not found");

            await _userManager.SetLockoutEndDateAsync(user, null);
            await _userManager.UpdateAsync(user);
        }

      
    }
}
