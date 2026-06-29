using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Contracts.Interface.Repository
{
    public interface IPermissionRepository
    {
        Task AssignPermissionAsync(string roleName, Guid permissionId);

        Task RemovePermissionAsync(string roleName, Guid permissionId);

        Task<List<string>> GetPermissionsAsync(string roleName);
        Task<List<string>> GetPermissionsByUserAsync(string userId);
    }
}
