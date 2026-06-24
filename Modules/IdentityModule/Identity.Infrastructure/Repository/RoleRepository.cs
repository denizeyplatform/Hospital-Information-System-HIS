using Identity.Application.Contracts.Interface.Repository;
using Identity.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Repository
{ 
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        public RoleRepository(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<string> CreateRoleAsync(string roleName, string description)
        {
            var role =
                await _roleManager.FindByNameAsync(roleName);

            if (role != null)
                throw new Exception("Role already exists");

            role = new Role
            {
                Name = roleName,
                Description = description
            };

            var result =
                await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
                throw new Exception(
                    string.Join(",",
                    result.Errors.Select(x => x.Description)));

            return role.Id;
        }

        public async Task AssignRoleAsync(string userId, string roleName)
        {
            var user =
                await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            if (await _userManager.IsInRoleAsync(user, roleName))
                return;

            await _userManager.AddToRoleAsync(
                user,
                roleName);


        }

        public async Task RemoveRoleAsync(string userId, string roleName)
        {
            var user =
                await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            await _userManager.RemoveFromRoleAsync(
                user,
                roleName);
        }

        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var user =
                await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            return (await _userManager.GetRolesAsync(user))
                .ToList();
        }

        public async Task AddClaimAsync(string roleName,string claimType,string claimValue)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
                throw new Exception("Role not found");

            var result = await _roleManager.AddClaimAsync(role,new Claim(claimType, claimValue));
            if (!result.Succeeded)
            {
                throw new Exception("No Claims assigned to the role");
            }
        }

        public async Task RemoveClaimAsync(string roleName,string claimType,string claimValue)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            await _roleManager.RemoveClaimAsync(role,new Claim(claimType, claimValue));
        }


    }
}
