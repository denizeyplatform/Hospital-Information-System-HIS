using Identity.Application.Contracts.Interface;
using Identity.Application.Contracts.Interface.Repository;
using Identity.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Contracts.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<string> CreateRoleAsync(CreateRoleRequest request)
        {
            return await _roleRepository.CreateRoleAsync(request.RoleName,request.Description);
        }
        public async Task AddClaimAsync(string roleName, RoleClaimRequest request)
        {
            await _roleRepository.AddClaimAsync(roleName, request.ClaimType, request.ClaimValue);
        }

        public async Task RemoveClaimAsync(string roleName, RoleClaimRequest request)
        {
            await _roleRepository.RemoveClaimAsync(roleName, request.ClaimType, request.ClaimValue);
        }

        public async Task AssignRoleAsync(string userId,AssignRoleRequest request)
        {
            await _roleRepository.AssignRoleAsync(userId, request.RoleName);
        }
        public async Task RemoveRoleAsync(string userId, string roleName)
        {
            await _roleRepository.RemoveRoleAsync(userId, roleName);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
        {
            return await _roleRepository.GetUserRolesAsync(userId);
        }

    }
}
