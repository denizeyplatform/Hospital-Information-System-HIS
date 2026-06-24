using Identity.Domain.Aggregate;
using Identity.Domain.Entities;
using Identity.Domain.Interface;
using Identity.Infrastructure.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Presistance.Data
{
    public class AppIdentityDBContext : IdentityDbContext<User, Role, string>
    {
        private readonly IDomainEventDispatcher _dispatcher;

        public AppIdentityDBContext(DbContextOptions<AppIdentityDBContext> options, IDomainEventDispatcher dispatcher) : base(options)
        {
           _dispatcher = dispatcher;
        }

        public async Task dispachEvents(CancellationToken cancellationToken = default)
        {
            var domainEvents = ChangeTracker
                .Entries<AggregateRoot<Guid>>()
                .Select(x => x.Entity)
                .SelectMany(x => x.DomainEvents)
                .ToList();


            await _dispatcher.DispatchAsync(domainEvents);

            foreach (var entity in ChangeTracker
                         .Entries<AggregateRoot<Guid>>())
            {
                entity.Entity.ClearDomainEvents();
            }

        }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermissionsModel> RolePermissions { get; set; }
    }
}
