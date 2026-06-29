using Identity.Domain.Constants;
using Identity.Domain.Entities;
using Identity.Infrastructure.Models;
using Identity.Infrastructure.Presistance.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Presistance.Seeding
{
    public static class PermissionSeeder
    {
        public static readonly RoleManager<Role> roleManager;

        public static async Task SeedAsync(AppIdentityDBContext context)
        {
            var permissions = GetAllPermissions();

            foreach (var permission in permissions)
            {
                var exists = await context.Permissions
                    .AnyAsync(x => x.Name == permission);

                if (!exists)
                {
                    await context.Permissions.AddAsync(
                        new Permission
                        {
                            Id = Guid.NewGuid(),
                            Name = permission,
                            Description = permission
                        });
                }
            }

            await context.SaveChangesAsync();

          // await AssignAdminRole(context);
        }

        public static async Task AssignAdminRole(AppIdentityDBContext context)
        {
            var adminRole = await roleManager.FindByNameAsync("Admin");

            var permissions =
                await context.Permissions.ToListAsync();

            foreach (var permission in permissions)
            {
                var exists =
                    await context.RolePermissions.AnyAsync(x =>
                        x.RoleId == adminRole.Id &&
                        x.PermissionId == permission.Id);

                if (!exists)
                {
                    context.RolePermissions.Add(
                        new RolePermissionsModel
                        {
                            RoleId = adminRole.Id,
                            PermissionId = permission.Id
                        });
                }
            }

            await context.SaveChangesAsync();
        }

        private static List<string> GetAllPermissions()
        {
            var permissionType = typeof(Permissions);

            return permissionType
                .GetNestedTypes()
                .SelectMany(t =>
                    t.GetFields(
                        BindingFlags.Public |
                        BindingFlags.Static |
                        BindingFlags.FlattenHierarchy))
                .Where(f =>
                    f.IsLiteral &&
                    !f.IsInitOnly &&
                    f.FieldType == typeof(string))
                .Select(f => f.GetRawConstantValue()!.ToString()!)
                .ToList();
        }
    }
}
