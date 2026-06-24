using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Contracts.Interface.Repository
{
    public interface IRoleRepository
    {
        Task<string> CreateRoleAsync(string roleName,string description);

        Task AssignRoleAsync(string userId,string roleName);

        Task RemoveRoleAsync(string userId,string roleName);

        Task<List<string>> GetUserRolesAsync(string userId);

        Task AddClaimAsync(string roleName,string claimType,string claimValue);

        Task RemoveClaimAsync(string roleName,string claimType,string claimValue);

   
    }
}
