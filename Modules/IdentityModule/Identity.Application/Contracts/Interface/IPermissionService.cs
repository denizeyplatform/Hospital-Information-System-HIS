using Identity.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Contracts.Interface
{
    public interface IPermissionService
    {
        Task AssignPermissionAsync(AssignPermissionDTO dto);

        Task RemovePermissionAsync(AssignPermissionDTO dto);

        Task<List<string>> GetPermissionsAsync(string rolename);
    }
}
