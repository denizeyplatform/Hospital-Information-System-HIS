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
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task AssignPermissionAsync(AssignPermissionDTO dto)
        {
            await _permissionRepository.AssignPermissionAsync(dto.RoleName, Guid.Parse(dto.PermissionId));
        }

        public async Task RemovePermissionAsync(AssignPermissionDTO dto)
        {
            await _permissionRepository.RemovePermissionAsync(dto.RoleName, Guid.Parse(dto.PermissionId));
        }
        public async Task<List<string>> GetPermissionsAsync(string rolename)
        {
            return await _permissionRepository.GetPermissionsAsync(rolename);
        }
    }
}
