using HIS.Domain.Aggregates.PatientAggregate.Entities;
using HIS.Domain.Aggregates.PatientAggregate.Entities.SubEntities;
using HIS.Domain.Common;
using HIS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Infrastructure.Presestance
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                IDomainEventDispatcher dispatcher) : base(options)
        {
            _dispatcher = dispatcher;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime>("CreatedAt");

                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime?>("UpdatedAt");
            }
        }

        public override async Task<int> SaveChangesAsync(
       CancellationToken cancellationToken = default)
        {
            var domainEvents = ChangeTracker
                .Entries<AggregateRoot<Guid>>()
                .Select(x => x.Entity)
                .SelectMany(x => x.DomainEvents)
                .ToList();

            var result = await base.SaveChangesAsync(
                cancellationToken);

            await _dispatcher.DispatchAsync(domainEvents); 

            foreach (var entity in ChangeTracker
                         .Entries<AggregateRoot<Guid>>())
            {
                entity.Entity.ClearDomainEvents();
            }

            return result;
        }




        public DbSet<Patient> Patients => Set<Patient>();
       // public DbSet<Appointment> Appointments => Set<Appointment>();

        public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();

        public DbSet<InsurancePolicy> InsurancePolicies => Set<InsurancePolicy>();

        public DbSet<DocumentRecord> Documents => Set<DocumentRecord>();

        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        public DbSet<EmergencyContact> EmergencyContacts => Set<EmergencyContact>();
    }
}
