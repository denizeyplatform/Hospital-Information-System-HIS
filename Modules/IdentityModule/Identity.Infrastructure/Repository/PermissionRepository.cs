using Identity.Application.Contracts.Interface.Repository;
using Identity.Domain.Entities;
using Identity.Infrastructure.Models;
using Identity.Infrastructure.Presistance.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly AppIdentityDBContext _dbContext;
        private readonly RoleManager<Role> _roleManager;
        public PermissionRepository(AppIdentityDBContext dbContext, RoleManager<Role> roleManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
        }
        public async Task AssignPermissionAsync(string roleName, Guid permissionId)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
                throw new Exception("Role not found");

            var exists = await _dbContext.RolePermissions
                .AnyAsync(rp => rp.RoleId == role.Id && rp.PermissionId == permissionId);

            if (exists)
                return;

            await _dbContext.RolePermissions.AddAsync(
                new RolePermissionsModel
                {
                    RoleId = role.Id,
                    PermissionId = permissionId
                });

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemovePermissionAsync(string roleName, Guid permissionId)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            var rolePermission = await _dbContext.RolePermissions
                .FirstOrDefaultAsync(rp =>
                    rp.RoleId == role.Id &&
                    rp.PermissionId == permissionId);


            if (rolePermission == null)
                throw new Exception("Permission is not assigned to this role.");

            _dbContext.RolePermissions.Remove(rolePermission);

            await _dbContext.SaveChangesAsync(); 
        }



        public async Task<List<string>> GetPermissionsAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            return await _dbContext.RolePermissions
            .AsNoTracking()
            .Where(x => x.RoleId == role.Id)
            .Select(x => x.Permission.Name)
            .ToListAsync();
        }

        public async Task<List<string>> GetPermissionsByUserAsync(string userId)
        {
            return await (
                from userRole in _dbContext.UserRoles
                join rolePermission in _dbContext.RolePermissions
                    on userRole.RoleId equals rolePermission.RoleId
                join permission in _dbContext.Permissions
                    on rolePermission.PermissionId equals permission.Id
                where userRole.UserId == userId
                select permission.Name
            )
            .Distinct()
            .ToListAsync();
        }
    }
}

