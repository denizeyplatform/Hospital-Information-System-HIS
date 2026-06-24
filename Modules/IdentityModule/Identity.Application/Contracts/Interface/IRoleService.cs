using Identity.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Contracts.Interface
{
    public interface IRoleService
    {
        Task<string> CreateRoleAsync(CreateRoleRequest request);
        Task AddClaimAsync(string roleName, RoleClaimRequest request);
        Task RemoveClaimAsync(string roleName, RoleClaimRequest request);
        Task AssignRoleAsync(string userId,AssignRoleRequest request);
        Task RemoveRoleAsync(string userId, string roleName);
        Task<IEnumerable<string>> GetUserRolesAsync(string userId);
    }
}
